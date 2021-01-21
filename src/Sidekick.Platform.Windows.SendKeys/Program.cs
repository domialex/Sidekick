using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

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
        static async void Main(string[] args)
        {
            var pid = -1;
            var keys = "";

            for (var i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("-pid:", StringComparison.Ordinal))
                {
                    pid = int.Parse(args[i][5..]);
                }
                else
                {
                    keys = args[i];
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

            foreach (var key in keys.Split('}'))
            {
                if (key.Contains('{', StringComparison.Ordinal))
                {
                    System.Windows.Forms.SendKeys.Send(key + "}");
                }
                else
                {
                    System.Windows.Forms.SendKeys.Send(key);
                }
                await Task.Delay(100);
            }
        }

        private static void WriteError(string message)
        {
            AttachConsole(-1);
            Console.WriteLine("ERROR LOL: " + message);
            FreeConsole();
        }
    }
}
