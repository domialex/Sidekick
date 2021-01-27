using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using Sidekick.Domain.Platforms;
using Sidekick.Platform.Windows.DllImport;

namespace Sidekick.Platform.Windows.Screen
{
    public class ScreenProvider : IScreenProvider
    {
        private readonly IMouseProvider mouse;

        public ScreenProvider(
            IMouseProvider mouse)
        {
            this.mouse = mouse;
        }

        public void Initialize()
        {
            // Nothing to do
        }

        private static List<Rectangle> Screens
        {
            get
            {
                var screens = new List<Rectangle>();

                User32.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, delegate (IntPtr hMonitor, IntPtr hdcMonitor, ref RectStruct lprcMonitor, IntPtr dwData)
                    {
                        var mi = new MonitorInfoEx();
                        mi.Size = Marshal.SizeOf(mi);
                        var success = User32.GetMonitorInfo(hMonitor, ref mi);
                        if (success)
                        {
                            screens.Add(new Rectangle(
                                mi.Monitor.Left,
                                mi.Monitor.Top,
                                mi.Monitor.Right - mi.Monitor.Left,
                                mi.Monitor.Bottom - mi.Monitor.Top
                            ));
                        }
                        return true;
                    }, IntPtr.Zero);

                return screens;
            }
        }

        public Rectangle GetBounds()
        {
            var screens = Screens;
            var mousePosition = mouse.GetPosition();

            if (screens.Any(x => x.Contains(mousePosition)))
            {
                return screens.First(x => x.Contains(mousePosition));
            }

            return screens.First();
        }
    }

}
