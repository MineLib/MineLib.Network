namespace MineLib.Network.Data
{
    /// <summary>
    ///     Represents the size of an object in 2D space.
    /// </summary>
    public struct Size2
    {
        public double Depth;
        public double Width;

        public Size2(double width, double depth)
        {
            Width = width;
            Depth = depth;
        }

        public Size2(Size2 s)
        {
            Width = s.Width;
            Depth = s.Depth;
        }

        /// <summary>
        /// Converts this Size to a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Depth: {0}, Width: {2}", Depth, Width);
        }

        // TODO: More operators
        public static Size2 operator /(Size2 a, double b)
        {
            return new Size2(a.Width/b, a.Depth/b);
        }
    }
}