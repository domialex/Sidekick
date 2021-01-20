using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Sidekick.Platform.Windows.SendKeys
{
    class Program
    {
        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);

        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool FreeConsole();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var pid = -1;
            var keysToSend = "";

            for (var i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("-pid:", StringComparison.Ordinal))
                {
                    int.TryParse(args[i][5..], out pid);
                }
                else
                {
                    keysToSend = args[i];
                }
            }

            if (pid != -1)
            {
                Process process;
                try
                {
                    process = Process.GetProcessById(pid);
                }
                catch (Exception ex)
                {
                    WriteError(ex.ToString());
                    return;
                }

                if (process.MainWindowHandle == IntPtr.Zero)
                {
                    WriteError($"Process {process.ProcessName} ({process.Id}) has no main window handle.");
                }
                else
                {
                    SetForegroundWindow(process.MainWindowHandle);
                }
            }

            System.Windows.Forms.SendKeys.SendWait(keysToSend);
        }

        private static void WriteError(string message)
        {
            AttachConsole(-1);
            Console.WriteLine("ERROR LOL: " + message);
            FreeConsole();
        }
    }
}
