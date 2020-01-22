using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace Sidekick.Helpers.NativeMethods
{
    public static class ClipboardHelper
    {
        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsClipboardFormatAvailable(uint format);

        [DllImport("User32.dll", SetLastError = true)]
        private static extern IntPtr GetClipboardData(uint uFormat);

        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseClipboard();

        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("Kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GlobalUnlock(IntPtr hMem);

        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern int GlobalSize(IntPtr hMem);

        private const uint CF_UNICODETEXT = 13U;

        public static string GetText()
        {
            if (!IsClipboardFormatAvailable(CF_UNICODETEXT))
            {
                return null;
            }

            IntPtr handle, pointer = default;

            try
            {
                if (!OpenClipboard(default))
                {
                    return null;
                }

                handle = GetClipboardData(CF_UNICODETEXT);
                if (handle == default)
                {
                    return null;
                }

                try
                {
                    pointer = GlobalLock(handle);
                    if (pointer == default)
                    {
                        return null;
                    }

                    var size = GlobalSize(handle);
                    var buffer = new byte[size];

                    Marshal.Copy(pointer, buffer, 0, size);

                    return Encoding.Unicode.GetString(buffer).TrimEnd('\0');
                }
                finally
                {
                    if (pointer != default)
                    {
                        GlobalUnlock(handle);
                    }
                }
            }
            finally
            {
                CloseClipboard();
            }
        }

        public static async void SetText(string text)
        {
            if (text == null)
                text = string.Empty;

            if(Thread.CurrentThread != Dispatcher.CurrentDispatcher.Thread)
            {
                await Dispatcher.CurrentDispatcher.InvokeAsync(() => SetText(text));
                return;
            }

            System.Windows.Clipboard.SetText(text);
        }

        public static async void SetDataObject(object data)
        {
            if (data == null)
                data = string.Empty;

            if (Thread.CurrentThread != Program.MAIN_DISPATCHER.Thread)
            {
                await Program.MAIN_DISPATCHER.InvokeAsync(() => SetDataObject(data));
                return;
            }

            System.Windows.Clipboard.SetDataObject(data);
        }
    }
}
