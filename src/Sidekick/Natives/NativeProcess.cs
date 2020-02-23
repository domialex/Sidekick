using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sidekick.Core.Extensions;
using Sidekick.Core.Initialization;
using Sidekick.Core.Loggers;
using Sidekick.Core.Natives;

namespace Sidekick.Natives
{
    public class NativeProcess : INativeProcess, IOnAfterInit
    {
        #region DllImport
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out Rectangle lpRect);

        [DllImport("user32.dll")]
        static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);
        #endregion

        private const string PATH_OF_EXILE_PROCESS_TITLE = "Path of Exile";
        private static readonly List<string> PossibleProcessNames = new List<string> { "PathOfExile", "PathOfExile_x64", "PathOfExileSteam", "PathOfExile_x64Steam" };


        private readonly string clientLogFileName = "Client.txt";
        private readonly string clientLogFolderName = "logs";

        public string ClientLogPath
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(GetPathOfExileProcess().GetMainModuleFileName()), clientLogFolderName, clientLogFileName);
            }
        }

        private const int STANDARD_RIGHTS_REQUIRED = 0xF0000;
        private const int TOKEN_ASSIGN_PRIMARY = 0x1;
        private const int TOKEN_DUPLICATE = 0x2;
        private const int TOKEN_IMPERSONATE = 0x4;
        private const int TOKEN_QUERY = 0x8;
        private const int TOKEN_QUERY_SOURCE = 0x10;
        private const int TOKEN_ADJUST_GROUPS = 0x40;
        private const int TOKEN_ADJUST_PRIVILEGES = 0x20;
        private const int TOKEN_ADJUST_SESSIONID = 0x100;
        private const int TOKEN_ADJUST_DEFAULT = 0x80;
        private const int TOKEN_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED | TOKEN_ASSIGN_PRIMARY | TOKEN_DUPLICATE | TOKEN_IMPERSONATE | TOKEN_QUERY | TOKEN_QUERY_SOURCE | TOKEN_ADJUST_PRIVILEGES | TOKEN_ADJUST_GROUPS | TOKEN_ADJUST_SESSIONID | TOKEN_ADJUST_DEFAULT;

        private readonly ILogger logger;

        public NativeProcess(ILogger logger)
        {
            this.logger = logger;
        }

        public Mutex Mutex { get; set; }

        public bool IsPathOfExileInFocus => ActiveWindowTitle == PATH_OF_EXILE_PROCESS_TITLE;

        public bool IsSidekickInFocus
        {
            get
            {
                var activatedHandle = GetForegroundWindow();
                if (activatedHandle == IntPtr.Zero)
                {
                    return false;
                }

                var procId = Process.GetCurrentProcess().Id;
                GetWindowThreadProcessId(activatedHandle, out var activeProcId);

                return activeProcId == procId;
            }
        }

        private string ActiveWindowTitle => GetActiveWindowProcess()?.MainWindowTitle;

        private Process GetActiveWindowProcess()
        {
            GetWindowThreadProcessId(GetForegroundWindow(), out var processID);
            return Process.GetProcessById(processID);
        }

        private Process GetPathOfExileProcess()
        {
            foreach (var processName in PossibleProcessNames)
            {
                var process = Process.GetProcessesByName(processName).FirstOrDefault();
                if (process != null)
                {
                    return process;
                }
            }

            return null;
        }

        public float ActiveWindowDpi
        {
            get
            {
                using var g = Graphics.FromHwnd(GetForegroundWindow());
                return g.DpiX;
            }
        }

        public Rectangle GetScreenDimensions()
        {
            return GetWindowRect(GetForegroundWindow(), out var rectangle) ? rectangle : default;
        }

        public async Task CheckPermission()
        {
            await WaitForPathOfExileFocus();

            if (IsUserRunAsAdmin() ? false : IsPathOfExileRunAsAdmin())
            {
                RestartAsAdmin();
            }
            else
            {
                logger.Log("Permission Sufficient.");
            }
        }

        private async Task WaitForPathOfExileFocus(int timeout = 200)
        {
            while (IsPathOfExileInFocus == false)
            {
                await Task.Delay(timeout);
            }
        }

        private void RestartAsAdmin()
        {
            var message = "This application must be run as administrator.";
            logger.Log(message, LogState.Error);

            if (MessageBox.Show(message + "\nClick Yes will restart as administrator automatically.", "Sidekick", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Mutex?.Close();
                try
                {
                    using var p = new Process();
                    p.StartInfo.FileName = Application.ExecutablePath;
                    p.StartInfo.UseShellExecute = true;
                    p.StartInfo.Verb = "runas";
                    p.Start();
                }
                catch (Win32Exception e)
                {
                    const int ERROR_CANCELLED = 1223; //The operation was canceled by the user.

                    if (e.NativeErrorCode == ERROR_CANCELLED)
                        MessageBox.Show("This application must be run as administrator.");
                    else
                        throw;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                finally
                {
                    Environment.Exit(Environment.ExitCode);
                }
            }
        }

        private bool IsUserRunAsAdmin()
        {
            var info = WindowsIdentity.GetCurrent();
            var principle = new WindowsPrincipal(info);
            return principle.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private bool IsPathOfExileRunAsAdmin()
        {
            var result = false;
            try
            {
                var proc = GetActiveWindowProcess();
                OpenProcessToken(proc.Handle, TOKEN_ALL_ACCESS, out var ph);
                using (var iden = new WindowsIdentity(ph))
                {
                    foreach (var role in iden.Groups)
                    {
                        if (role.IsValidTargetType(typeof(SecurityIdentifier)))
                        {
                            var sid = role as SecurityIdentifier;

                            if (sid.IsWellKnown(WellKnownSidType.AccountAdministratorSid) || sid.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid))
                            {
                                result = true;
                                break;
                            }
                        }
                    }

                    CloseHandle(ph);
                }

                return result;
            }
            catch (Exception e)
            {
                logger.Log(e.Message, LogState.Error);
                RestartAsAdmin();
            }

            return result;
        }

        public Task OnAfterInit()
        {
            Task.Run(CheckPermission);
            return Task.CompletedTask;
        }
    }
}
