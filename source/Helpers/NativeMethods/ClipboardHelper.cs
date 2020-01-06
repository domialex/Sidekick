using System;
using System.Runtime.InteropServices;
using System.Text;

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
    }
}
