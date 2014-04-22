namespace MineLib.Network.Data
{
    /// <summary>
    ///     Represents the size of an object in 3D space.
    /// </summary>
    public struct Size
    {
        public double Depth;
        public double Height;
        public double Width;

        public Size(double width, double height, double depth)
        {
            Width = width;
            Height = height;
            Depth = depth;
        }

        public Size(Size s)
        {
            Width = s.Width;
            Height = s.Height;
            Depth = s.Depth;
        }

        /// <summary>
        /// Converts this Size to a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Depth: {0}, Height: {1}, Width: {2}", Depth, Height, Width);
        }

        // TODO: More operators
        public static Size operator /(Size a, double b)
        {
            return new Size(a.Width/b, a.Height/b, a.Depth/b);
        }
    }
}