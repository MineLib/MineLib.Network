using System;
using System.Runtime.InteropServices;

namespace MineLib.Network.Data
{
    /// <summary>
    /// Represents the location of an object in 2D space.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct Vector2 : IEquatable<Vector2>
    {
        [FieldOffset(0)]
        public double X;
        [FieldOffset(8)]
        public double Z;

        public Vector2(double value)
        {
            X = Z = value;
        }

        public Vector2(double x, double z)
        {
            X = x;
            Z = z;
        }

        public Vector2(Vector2 v)
        {
            X = v.X;
            Z = v.Z;
        }

        /// <summary>
        /// Converts this Vector2 to a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("X: {0}, Z: {1}", X, Z);
        }

        #region Math

        /// <summary>
        /// Truncates the decimal component of each part of this Vector2.
        /// </summary>
        public Vector2 Floor()
        {
            return new Vector2(Math.Floor(X), Math.Floor(Z));
        }

        /// <summary>
        /// Calculates the distance between two Vector2 objects.
        /// </summary>
        public double DistanceTo(Vector2 other)
        {
            return Math.Sqrt(Square(other.X - X) +
                             Square(other.Z - Z));
        }

        /// <summary>
        /// Calculates the square of a num.
        /// </summary>
        private double Square(double num)
        {
            return num * num;
        }

        /// <summary>
        /// Finds the distance of this vector from Vector2.Zero
        /// </summary>
        public double Distance
        {
            get
            {
                return DistanceTo(Zero);
            }
        }

        public static Vector2 Min(Vector2 value1, Vector2 value2)
        {
            return new Vector2(
                Math.Min(value1.X, value2.X),
                Math.Min(value1.Z, value2.Z)
                );
        }

        public static Vector2 Max(Vector2 value1, Vector2 value2)
        {
            return new Vector2(
                Math.Max(value1.X, value2.X),
                Math.Max(value1.Z, value2.Z)
                );
        }

        #endregion

        #region Operators

        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return !a.Equals(b);
        }

        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return a.Equals(b);
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Z + b.Z);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Z - b.Z);
        }

        public static Vector2 operator +(Vector2 a, Size2 b)
        {
            return new Vector2(
                a.X + b.Width,
                a.Z + b.Depth);
        }

        public static Vector2 operator -(Vector2 a, Size2 b)
        {
            return new Vector2(
                a.X - b.Width,
                a.Z - b.Depth);
        }

        public static Vector2 operator -(Vector2 a)
        {
            return new Vector2(
                -a.X,
                -a.Z);
        }

        public static Vector2 operator *(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X * b.X, a.Z * b.Z);
        }

        public static Vector2 operator /(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X / b.X, a.Z / b.Z);
        }

        public static Vector2 operator %(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X % b.X, a.Z % b.Z);
        }

        public static Vector2 operator +(Vector2 a, double b)
        {
            return new Vector2(a.X + b, a.Z + b);
        }

        public static Vector2 operator -(Vector2 a, double b)
        {
            return new Vector2(a.X - b, a.Z - b);
        }

        public static Vector2 operator *(Vector2 a, double b)
        {
            return new Vector2(a.X * b, a.Z * b);
        }

        public static Vector2 operator /(Vector2 a, double b)
        {
            return new Vector2(a.X / b, a.Z / b);
        }

        public static Vector2 operator %(Vector2 a, double b)
        {
            return new Vector2(a.X % b, a.Z % b);
        }

        public static Vector2 operator +(double a, Vector2 b)
        {
            return new Vector2(a + b.X, a + b.Z);
        }

        public static Vector2 operator -(double a, Vector2 b)
        {
            return new Vector2(a - b.X, a - b.Z);
        }

        public static Vector2 operator *(double a, Vector2 b)
        {
            return new Vector2(a * b.X, a * b.Z);
        }

        public static Vector2 operator /(double a, Vector2 b)
        {
            return new Vector2(a / b.X, a / b.Z);
        }

        public static Vector2 operator %(double a, Vector2 b)
        {
            return new Vector2(a % b.X, a % b.Z);
        }

        public static explicit operator Vector2(Vector3 a)
        {
            return new Vector2(a.X, a.Z);
        }

        #endregion

        #region Constants

        public static Vector2 Zero
        {
            get { return new Vector2(0); }
        }

        public static Vector2 One
        {
            get { return new Vector2(1); }
        }

        public static Vector2 Forwards
        {
            get { return new Vector2(0, 1); }
        }

        public static Vector2 Backwards
        {
            get { return new Vector2(0, -1); }
        }

        public static Vector2 Left
        {
            get { return new Vector2(-1, 0); }
        }

        public static Vector2 Right
        {
            get { return new Vector2(1, 0); }
        }

        public static Vector2 South
        {
            get { return new Vector2(0, 1); }
        }

        public static Vector2 North
        {
            get { return new Vector2(0, -1); }
        }

        public static Vector2 West
        {
            get { return new Vector2(-1, 0); }
        }

        public static Vector2 East
        {
            get { return new Vector2(1, 0); }
        }

        #endregion

        public bool Equals(Vector2 other)
        {
            return other.X.Equals(X) && other.Z.Equals(Z);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Vector2)) return false;
            return Equals((Vector2)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = X.GetHashCode();
                result = (result * 397) ^ Z.GetHashCode();
                return result;
            }
        }
    }

}