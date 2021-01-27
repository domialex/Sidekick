namespace GregsStack.InputSimulatorStandard.Native
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct Point
    {
        /// <summary>
        /// Gets or sets the x-coordinate of this Point.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the y-coordinate of this Point.
        /// </summary>
        public int Y { get; set; }

        public static implicit operator System.Drawing.Point(Point point)
        {
            return new System.Drawing.Point(point.X, point.Y);
        }
    }
}
