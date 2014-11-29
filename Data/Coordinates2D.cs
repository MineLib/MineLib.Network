using System;
using MineLib.Network.IO;

namespace MineLib.Network.Data
{
    /// <summary>
    /// Represents mostly Chunk coordinates
    /// </summary>
    public struct Coordinates2D : IEquatable<Coordinates2D>
    {
        public readonly int X;
        public readonly int Z;

        public Coordinates2D(int value)
        {
            X = Z = value;
        }

        public Coordinates2D(int x, int z)
        {
            X = x;
            Z = z;
        }

        public Coordinates2D(Coordinates2D v)
        {
            X = v.X;
            Z = v.Z;
        }

        #region Network

        public static Coordinates2D FromReaderVarInt(IMinecraftDataReader reader)
        {
            return new Coordinates2D
            (
                reader.ReadVarInt(),
                reader.ReadVarInt()
            );
        }

        public static Coordinates2D FromReaderByte(IMinecraftDataReader reader)
        {
            return new Coordinates2D
            (
                reader.ReadByte(),
                reader.ReadByte()
            );
        }

        public static Coordinates2D FromReaderShort(IMinecraftDataReader reader)
        {
            return new Coordinates2D
            (
                reader.ReadShort(),
                reader.ReadShort()
            );
        }

        public static Coordinates2D FromReaderInt(IMinecraftDataReader reader)
        {
            return new Coordinates2D
            (
                reader.ReadInt(),
                reader.ReadInt()
            );
        }


        public void ToStreamVarInt(IMinecraftStream stream)
        {
            stream.WriteVarInt(X);
            stream.WriteVarInt(Z);
        }

        public void ToStreamByte(IMinecraftStream stream)
        {
            stream.WriteByte((byte) X);
            stream.WriteByte((byte) Z);
        }

        public void ToStreamShort(IMinecraftStream stream)
        {
            stream.WriteShort((short) X);
            stream.WriteShort((short) Z);
        }

        public void ToStreamInt(IMinecraftStream stream)
        {
            stream.WriteInt(X);
            stream.WriteInt(Z);
        }

        #endregion

        /// <summary>
        /// Converts this Coordinates2D to a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("X: {0}, Z: {1}", X, Z);
        }

        #region Math

        /// <summary>
        /// Calculates the distance between two Coordinates2D objects.
        /// </summary>
        public double DistanceTo(Coordinates2D other)
        {
            return Math.Sqrt(Square(other.X - X) +
                             Square(other.Z - Z));
        }

        /// <summary>
        /// Calculates the square of a num.
        /// </summary>
        private static int Square(int num)
        {
            return num * num;
        }

        /// <summary>
        /// Finds the distance of this Coordinates2D from Coordinates2D.Zero
        /// </summary>
        public double Distance
        {
            get
            {
                return DistanceTo(Zero);
            }
        }

        public static Coordinates2D Min(Coordinates2D value1, Coordinates2D value2)
        {
            return new Coordinates2D(
                Math.Min(value1.X, value2.X),
                Math.Min(value1.Z, value2.Z)
                );
        }

        public static Coordinates2D Max(Coordinates2D value1, Coordinates2D value2)
        {
            return new Coordinates2D(
                Math.Max(value1.X, value2.X),
                Math.Max(value1.Z, value2.Z)
                );
        }

        #endregion

        #region Operators

        public static bool operator !=(Coordinates2D a, Coordinates2D b)
        {
            return !a.Equals(b);
        }

        public static bool operator ==(Coordinates2D a, Coordinates2D b)
        {
            return a.Equals(b);
        }

        public static Coordinates2D operator +(Coordinates2D a, Coordinates2D b)
        {
            return new Coordinates2D(a.X + b.X, a.Z + b.Z);
        }

        public static Coordinates2D operator -(Coordinates2D a, Coordinates2D b)
        {
            return new Coordinates2D(a.X - b.X, a.Z - b.Z);
        }

        public static Coordinates2D operator -(Coordinates2D a)
        {
            return new Coordinates2D(-a.X, -a.Z);
        }

        public static Coordinates2D operator *(Coordinates2D a, Coordinates2D b)
        {
            return new Coordinates2D(a.X * b.X, a.Z * b.Z);
        }

        public static Coordinates2D operator /(Coordinates2D a, Coordinates2D b)
        {
            return new Coordinates2D(a.X / b.X, a.Z / b.Z);
        }

        public static Coordinates2D operator %(Coordinates2D a, Coordinates2D b)
        {
            return new Coordinates2D(a.X % b.X, a.Z % b.Z);
        }

        public static Coordinates2D operator +(Coordinates2D a, int b)
        {
            return new Coordinates2D(a.X + b, a.Z + b);
        }

        public static Coordinates2D operator -(Coordinates2D a, int b)
        {
            return new Coordinates2D(a.X - b, a.Z - b);
        }

        public static Coordinates2D operator *(Coordinates2D a, int b)
        {
            return new Coordinates2D(a.X * b, a.Z * b);
        }

        public static Coordinates2D operator /(Coordinates2D a, int b)
        {
            return new Coordinates2D(a.X / b, a.Z / b);
        }

        public static Coordinates2D operator %(Coordinates2D a, int b)
        {
            return new Coordinates2D(a.X % b, a.Z % b);
        }

        public static Coordinates2D operator +(int a, Coordinates2D b)
        {
            return new Coordinates2D(a + b.X, a + b.Z);
        }

        public static Coordinates2D operator -(int a, Coordinates2D b)
        {
            return new Coordinates2D(a - b.X, a - b.Z);
        }

        public static Coordinates2D operator *(int a, Coordinates2D b)
        {
            return new Coordinates2D(a * b.X, a * b.Z);
        }

        public static Coordinates2D operator /(int a, Coordinates2D b)
        {
            return new Coordinates2D(a / b.X, a / b.Z);
        }

        public static Coordinates2D operator %(int a, Coordinates2D b)
        {
            return new Coordinates2D(a % b.X, a % b.Z);
        }

        public static explicit operator Coordinates2D(Position a)
        {
            return new Coordinates2D(a.X, a.Z);
        }

        #endregion

        #region Constants

        public static readonly Coordinates2D Zero = new Coordinates2D(0);
        public static readonly Coordinates2D One = new Coordinates2D(1);

        public static readonly Coordinates2D Forward = new Coordinates2D(0, 1);
        public static readonly Coordinates2D Backward = new Coordinates2D(0, -1);
        public static readonly Coordinates2D Left = new Coordinates2D(-1, 0);
        public static readonly Coordinates2D Right = new Coordinates2D(1, 0);

        #endregion

        public bool Equals(Coordinates2D other)
        {
            return other.X.Equals(X) && other.Z.Equals(Z);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Coordinates2D)) return false;
            return Equals((Coordinates2D)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var result = X.GetHashCode();
                result = (result * 397) ^ Z.GetHashCode();
                return result;
            }
        }
    }
}