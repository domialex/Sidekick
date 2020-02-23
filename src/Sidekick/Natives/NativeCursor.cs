using System.Drawing;
using System.Runtime.InteropServices;
using Sidekick.Core.Natives;

namespace Sidekick.Natives
{
    public class NativeCursor : INativeCursor
    {
        #region DllImport
        /// <summary>
        /// Retrieves the cursor's position, in screen coordinates.
        /// </summary>
        /// <see>See MSDN documentation for further information.</see>
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out Point lpPoint);
        #endregion

        public NativeCursor()
        {
        }

        public Point GetCursorPosition()
        {
            GetCursorPos(out var lpPoint);

            return lpPoint;
        }
    }
}
