using Sidekick.Core.Loggers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sidekick.Helpers.NativeMethods
{
    public static class ProcessHelper
    {
        public static Mutex mutex { private get; set; }

        private const string PATH_OF_EXILE_PROCESS_TITLE = "Path of Exile";

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
        private const int TOKEN_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | TOKEN_ASSIGN_PRIMARY | TOKEN_DUPLICATE | TOKEN_IMPERSONATE | TOKEN_QUERY | TOKEN_QUERY_SOURCE | TOKEN_ADJUST_PRIVILEGES | TOKEN_ADJUST_GROUPS | TOKEN_ADJUST_SESSIONID | TOKEN_ADJUST_DEFAULT);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hWnd, out Rectangle lpRect);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle, UInt32 DesiredAccess, out IntPtr TokenHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        public static bool IsPathOfExileInFocus()
        {
            return GetActiveWindowTitle() == PATH_OF_EXILE_PROCESS_TITLE;
        }

        public static string GetActiveWindowTitle()
        {
            return GetActiveWindowProcess()?.MainWindowTitle;
        }

        public static Process GetActiveWindowProcess()
        {
            GetWindowThreadProcessId(GetForegroundWindow(), out int processID);
            return Process.GetProcessById(processID);
        }

        public static float GetActiveWindowDpi()
        {
            using (Graphics g = Graphics.FromHwnd(GetForegroundWindow()))
            {
                return g.DpiX;
            }
        }

        public static int GetActiveWindowWidth()
        {
            GetWindowRect(GetForegroundWindow(), out Rectangle windowRect);
            return windowRect.Width;
        }

        async public static void CheckPermission()
        {
            await WaitForPathOfExileFocus();

            bool IsSufficient = IsPermissionSufficient();

            if (IsSufficient == false)
            {
                RestartAsAdmin();
            }
            else
            {
                Legacy.Logger.Log("Permission Sufficient.");
            }
        }

        public async static Task WaitForPathOfExileFocus(int timeout = 200)
        {
            while (IsPathOfExileInFocus() == false)
            {
                await Task.Delay(timeout);
            }
        }

        public static void RestartAsAdmin()
        {
            string message = "This application must be run as administrator.";
            Legacy.Logger.Log(message, LogState.Error);

            if (MessageBox.Show(message + "\nClick Yes will restart as administrator automatically.", "Sidekick", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                mutex?.Close();
                try
                {
                    using (Process p = new Process())
                    {
                        p.StartInfo.FileName = Application.ExecutablePath;
                        p.StartInfo.UseShellExecute = true;
                        p.StartInfo.Verb = "runas";
                        p.Start();
                    }
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

        public static bool IsPermissionSufficient()
        {
            return IsUserRunAsAdmin() ? true : !IsPathOfExileRunAsAdmin();
        }

        public static bool IsUserRunAsAdmin()
        {
            var info = WindowsIdentity.GetCurrent();
            var principle = new WindowsPrincipal(info);
            bool isAdmin = principle.IsInRole(WindowsBuiltInRole.Administrator);

            return isAdmin;
        }

        public static bool IsPathOfExileRunAsAdmin()
        {
            bool result = false;
            try
            {
                Process proc = GetActiveWindowProcess();
                OpenProcessToken(proc.Handle, TOKEN_ALL_ACCESS, out IntPtr ph);
                WindowsIdentity iden = new WindowsIdentity(ph);

                foreach (IdentityReference role in iden.Groups)
                {
                    if (role.IsValidTargetType(typeof(SecurityIdentifier)))
                    {
                        SecurityIdentifier sid = role as SecurityIdentifier;

                        if (sid.IsWellKnown(WellKnownSidType.AccountAdministratorSid) || sid.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid))
                        {
                            result = true;
                            break;
                        }
                    }
                }

                CloseHandle(ph);

                return result;
            }
            catch (Exception e)
            {
                Legacy.Logger.Log(e.Message, LogState.Error);
                RestartAsAdmin();
            }

            return result;
        }
    }
}
