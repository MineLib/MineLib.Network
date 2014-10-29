using System;
using MineLib.Network.IO;

// From https://github.com/SirCmpwn/Craft.Net
namespace MineLib.Network.Modern.Data
{
    /// <summary>
    /// Represents the location of an object in 3D space (double).
    /// </summary>
    public struct Vector3 : IEquatable<Vector3>
    {
        public double X, Y, Z;

        public Vector3(double value)
        {
            X = Y = Z = value;
        }

        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3(Vector3 v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        public static Vector3 FromFixedPoint(int x, int y, int z)
        {
            return new Vector3
            {
                X = x / 32.0,
                Y = y / 32.0,
                Z = z / 32.0
            };
        }

        #region Network

        public static Vector3 FromReaderByte(IMinecraftDataReader reader)
        {
            return new Vector3
            {
                X = reader.ReadByte(),
                Y = reader.ReadByte(),
                Z = reader.ReadByte()
            };
        }

        public static Vector3 FromReaderDouble(IMinecraftDataReader reader)
        {
            return new Vector3
            {
                X = reader.ReadDouble(),
                Y = reader.ReadDouble(),
                Z = reader.ReadDouble()
            };
        }

        public static Vector3 FromReaderSByteFixedPoint(IMinecraftDataReader reader)
        {
            return new Vector3
            {
                X = reader.ReadSByte() / 32.0,
                Y = reader.ReadSByte() / 32.0,
                Z = reader.ReadSByte() / 32.0
            };
        }

        public static Vector3 FromReaderIntFixedPoint(IMinecraftDataReader reader)
        {
            return new Vector3
            {
                X = reader.ReadInt() / 32.0,
                Y = reader.ReadInt() / 32.0,
                Z = reader.ReadInt() / 32.0
            };
        }


        public void ToStreamByte(IMinecraftStream stream)
        {
            stream.WriteByte((byte)X);
            stream.WriteByte((byte)Y);
            stream.WriteByte((byte)Z);
        }

        public void ToStreamDouble(IMinecraftStream stream)
        {
            stream.WriteDouble(X);
            stream.WriteDouble(Y);
            stream.WriteDouble(Z);
        }
        // TODO: Check that
        public void ToStreamSByteFixedPoint(IMinecraftStream stream)
        {
            stream.WriteSByte((sbyte) (X * 32));
            stream.WriteSByte((sbyte) (Y * 32));
            stream.WriteSByte((sbyte) (Z * 32));
        }
        // TODO: Check that
        public void ToStreamIntFixedPoint(IMinecraftStream stream)
        {
            stream.WriteInt((int) X * 32);
            stream.WriteInt((int) Y * 32);
            stream.WriteInt((int) Z * 32);
        }

        #endregion

        /// <summary>
        /// Converts this Vector3 to a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}, Z: {2}", X, Y, Z);
        }

        #region Math

        /// <summary>
        /// Truncates the decimal component of each part of this Vector3.
        /// </summary>
        public Vector3 Floor()
        {
            return new Vector3(Math.Floor(X), Math.Floor(Y), Math.Floor(Z));
        }

        /// <summary>
        /// Calculates the distance between two Vector3 objects.
        /// </summary>
        public double DistanceTo(Vector3 other)
        {
            return Math.Sqrt(Square(other.X - X) +
                             Square(other.Y - Y) +
                             Square(other.Z - Z));
        }

        /// <summary>
        /// Calculates the square of a num.
        /// </summary>
        private static double Square(double num)
        {
            return num * num;
        }

        /// <summary>
        /// Finds the distance of this vector from Vector3.Zero
        /// </summary>
        public double Distance
        {
            get
            {
                return DistanceTo(Zero);
            }
        }

        public static Vector3 Min(Vector3 value1, Vector3 value2)
        {
            return new Vector3(
                Math.Min(value1.X, value2.X),
                Math.Min(value1.Y, value2.Y),
                Math.Min(value1.Z, value2.Z)
                );
        }

        public static Vector3 Max(Vector3 value1, Vector3 value2)
        {
            return new Vector3(
                Math.Max(value1.X, value2.X),
                Math.Max(value1.Y, value2.Y),
                Math.Max(value1.Z, value2.Z)
                );
        }

        #endregion

        #region Operators

        public static bool operator !=(Vector3 a, Vector3 b)
        {
            return !a.Equals(b);
        }

        public static bool operator ==(Vector3 a, Vector3 b)
        {
            return a.Equals(b);
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(
                a.X + b.X,
                a.Y + b.Y,
                a.Z + b.Z);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(
                a.X - b.X,
                a.Y - b.Y,
                a.Z - b.Z);
        }

        public static Vector3 operator +(Vector3 a, Size b)
        {
            return new Vector3(
                a.X + b.Width,
                a.Y + b.Height,
                a.Z + b.Depth);
        }

        public static Vector3 operator -(Vector3 a, Size b)
        {
            return new Vector3(
                a.X - b.Width,
                a.Y - b.Height,
                a.Z - b.Depth);
        }

        public static Vector3 operator -(Vector3 a)
        {
            return new Vector3(
                -a.X,
                -a.Y,
                -a.Z);
        }

        public static Vector3 operator *(Vector3 a, Vector3 b)
        {
            return new Vector3(
                a.X * b.X,
                a.Y * b.Y,
                a.Z * b.Z);
        }

        public static Vector3 operator /(Vector3 a, Vector3 b)
        {
            return new Vector3(
                a.X / b.X,
                a.Y / b.Y,
                a.Z / b.Z);
        }

        public static Vector3 operator %(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X % b.X, a.Y % b.Y, a.Z % b.Z);
        }

        public static Vector3 operator +(Vector3 a, double b)
        {
            return new Vector3(
                a.X + b,
                a.Y + b,
                a.Z + b);
        }

        public static Vector3 operator -(Vector3 a, double b)
        {
            return new Vector3(
                a.X - b,
                a.Y - b,
                a.Z - b);
        }

        public static Vector3 operator *(Vector3 a, double b)
        {
            return new Vector3(
                a.X * b,
                a.Y * b,
                a.Z * b);
        }

        public static Vector3 operator /(Vector3 a, double b)
        {
            return new Vector3(
                a.X / b,
                a.Y / b,
                a.Z / b);
        }

        public static Vector3 operator %(Vector3 a, double b)
        {
            return new Vector3(a.X % b, a.Y % b, a.Y % b);
        }

        public static Vector3 operator +(double a, Vector3 b)
        {
            return new Vector3(
                a + b.X,
                a + b.Y,
                a + b.Z);
        }

        public static Vector3 operator -(double a, Vector3 b)
        {
            return new Vector3(
                a - b.X,
                a - b.Y,
                a - b.Z);
        }

        public static Vector3 operator *(double a, Vector3 b)
        {
            return new Vector3(
                a * b.X,
                a * b.Y,
                a * b.Z);
        }

        public static Vector3 operator /(double a, Vector3 b)
        {
            return new Vector3(
                a / b.X,
                a / b.Y,
                a / b.Z);
        }

        public static Vector3 operator %(double a, Vector3 b)
        {
            return new Vector3(a % b.X, a % b.Y, a % b.Y);
        }

        #endregion

        #region Constants

        public static readonly Vector3 Zero = new Vector3(0, 0, 0);
        public static readonly Vector3 One = new Vector3(1, 1, 1);

        public static readonly Vector3 Up = new Vector3(0, 1, 0);
        public static readonly Vector3 Down = new Vector3(0, -1, 0);
        public static readonly Vector3 Left = new Vector3(-1, 0, 0);
        public static readonly Vector3 Right = new Vector3(1, 0, 0);
        public static readonly Vector3 Backwards = new Vector3(0, 0, -1);
        public static readonly Vector3 Forwards = new Vector3(0, 0, 1);

        public static readonly Vector3 East = new Vector3(1, 0, 0);
        public static readonly Vector3 West = new Vector3(-1, 0, 0);
        public static readonly Vector3 North = new Vector3(0, 0, -1);
        public static readonly Vector3 South = new Vector3(0, 0, 1);

        #endregion

        public bool Equals(Vector3 other)
        {
            return other.X.Equals(X) && other.Y.Equals(Y) && other.Z.Equals(Z);
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Vector3))
                return false;

            return Equals((Vector3) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var result = X.GetHashCode();
                result = (result * 397) ^ Y.GetHashCode();
                result = (result * 397) ^ Z.GetHashCode();
                return result;
            }
        }
    }
}