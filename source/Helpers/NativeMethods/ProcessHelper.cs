using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Sidekick.Helpers.NativeMethods
{
    public static class ProcessHelper
    {
        private const string PATH_OF_EXILE_PROCESS_TITLE = "Path of Exile";

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hWnd, out Rectangle lpRect);

        public static bool IsPathOfExileInFocus()
        {
            //if (Debugger.IsAttached)
            //{
            //    return true;
            //}

            GetWindowThreadProcessId(GetForegroundWindow(), out int processID);
            var processToCheck = Process.GetProcessById(processID);

            return processToCheck?.MainWindowTitle == PATH_OF_EXILE_PROCESS_TITLE;
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
    }
}
