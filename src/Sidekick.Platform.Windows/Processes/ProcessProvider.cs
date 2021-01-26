using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Notifications.Commands;
using Sidekick.Domain.Platforms;

namespace Sidekick.Platform.Windows.Processes
{
    public class ProcessProvider : IProcessProvider
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

        private const string APPLICATION_PROCESS_GUID = "93c46709-7db2-4334-8aa3-28d473e66041";
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
        private readonly IMediator mediator;
        private readonly IStringLocalizer<ProcessProvider> localizer;

        public ProcessProvider(
            ILogger<ProcessProvider> logger,
            IMediator mediator,
            IStringLocalizer<ProcessProvider> localizer)
        {
            this.logger = logger;
            this.mediator = mediator;
            this.localizer = localizer;
        }

        public async Task Initialize(CancellationToken cancellationToken)
        {
            Mutex = new Mutex(true, APPLICATION_PROCESS_GUID, out var instanceResult);

            _ = Task.Run(CheckPermission, cancellationToken);

            if (!instanceResult)
            {
                await mediator.Send(new OpenNotificationCommand(localizer["AlreadyRunningText"]));
                Environment.Exit(Environment.ExitCode);
            }
        }

        private Mutex Mutex { get; set; }

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

                var procId = Environment.ProcessId;
                GetWindowThreadProcessId(activatedHandle, out var activeProcId);

                return activeProcId == procId;
            }
        }

        private static string ActiveWindowTitle => GetActiveWindowProcess()?.MainWindowTitle;

        private static Process GetActiveWindowProcess()
        {
            GetWindowThreadProcessId(GetForegroundWindow(), out var processID);
            return Process.GetProcessById(processID);
        }

        private static Process GetPathOfExileProcess()
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

        private async Task CheckPermission()
        {
            while (!IsPathOfExileInFocus)
            {
                await Task.Delay(1000);
            }

            if (!IsUserRunAsAdmin() && await IsPathOfExileRunAsAdmin())
            {
                await RestartAsAdmin();
            }
        }

        private async Task RestartAsAdmin()
        {
            logger.LogError(localizer["RestartText"]);

            await mediator.Send(new OpenConfirmNotificationCommand(localizer["RestartText"],
                onYes: async () =>
                {
                    Mutex?.Close();
                    try
                    {
                        using var p = new Process();
                        p.StartInfo.FileName = "Sidekick.exe";
                        // p.StartInfo.FileName = System.Windows.Forms.Application.ExecutablePath;
                        p.StartInfo.UseShellExecute = true;
                        p.StartInfo.Verb = "runas";
                        p.Start();
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, localizer["AdminError"]);
                        await mediator.Send(new OpenNotificationCommand(localizer["AdminError"]));
                    }
                    finally
                    {
                        Environment.Exit(Environment.ExitCode);
                    }
                }));
        }

        private static bool IsUserRunAsAdmin()
        {
            var info = WindowsIdentity.GetCurrent();
            var principle = new WindowsPrincipal(info);
            return principle.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private async Task<bool> IsPathOfExileRunAsAdmin()
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
                logger.LogError(e, e.Message);
                await RestartAsAdmin();
            }

            return result;
        }
    }
}
