using System;
using MineLib.Network.IO;

namespace MineLib.Network.Modern.Data
{
    /// <summary>
    /// Represents the location of an object in 3D space (int).
    /// </summary>
    public struct Position : IEquatable<Position>
    {
        public int X;
        public int Y;
        public int Z;

        public Position(int value)
        {
            X = Y = Z = value;
        }

        public Position(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Position(Position v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        public static Position FromLong(long value)
        {
            return new Position
            {
                X = (int) (value >> 38),
                Y = (int) (value >> 26) & 0xFFF,
                Z = (int) value << 38 >> 38
            };
        }

        public long ToLong()
        {
            return ((X & 0x3FFFFFF) << 38) | ((Y & 0xFFF) << 26) | (Z & 0x3FFFFFF);
        }

        #region Network

        public static Position FromReaderInt(MinecraftDataReader reader)
        {
            return new Position
            {
                X = reader.ReadInt(),
                Y = reader.ReadInt(),
                Z = reader.ReadInt()
            };
        }

        public static Position FromReaderVarInt(MinecraftDataReader reader)
        {
            return new Position
            {
                X = reader.ReadVarInt(),
                Y = reader.ReadVarInt(),
                Z = reader.ReadVarInt()
            };
        }

        public static Position FromReaderLong(MinecraftDataReader reader)
        {
            return FromLong(reader.ReadLong());
        }


        public void ToStreamInt(MinecraftStream stream)
        {
            stream.WriteInt(X);
            stream.WriteInt(Y);
            stream.WriteInt(Z);
        }

        public void ToStreamVarInt(MinecraftStream stream)
        {
            stream.WriteVarInt(X);
            stream.WriteVarInt(Y);
            stream.WriteVarInt(Z);
        }

        public void ToStreamLong(MinecraftStream stream)
        {
            stream.WriteLong(ToLong());
        }

        #endregion

        /// <summary>
        /// Converts this Position to a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}, Z: {2}", X, Y, Z);
        }

        #region Math

        /// <summary>
        /// Calculates the distance between two Position objects.
        /// </summary>
        public double DistanceTo(Position other)
        {
            return Math.Sqrt(Square(other.X - X) +
                             Square(other.Y - Y) +
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
        /// Finds the distance of this Position from Position.Zero
        /// </summary>
        public double Distance
        {
            get
            {
                return DistanceTo(Zero);
            }
        }

        public static Position Min(Position value1, Position value2)
        {
            return new Position(
                Math.Min(value1.X, value2.X),
                Math.Min(value1.Y, value2.Y),
                Math.Min(value1.Z, value2.Z)
                );
        }

        public static Position Max(Position value1, Position value2)
        {
            return new Position(
                Math.Max(value1.X, value2.X),
                Math.Max(value1.Y, value2.Y),
                Math.Max(value1.Z, value2.Z)
                );
        }

        #endregion

        #region Operators

        public static bool operator !=(Position a, Position b)
        {
            return !a.Equals(b);
        }

        public static bool operator ==(Position a, Position b)
        {
            return a.Equals(b);
        }

        public static Position operator +(Position a, Position b)
        {
            return new Position(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Position operator -(Position a, Position b)
        {
            return new Position(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Position operator -(Position a)
        {
            return new Position(-a.X, -a.Y, -a.Z);
        }

        public static Position operator *(Position a, Position b)
        {
            return new Position(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        }

        public static Position operator /(Position a, Position b)
        {
            return new Position(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
        }

        public static Position operator %(Position a, Position b)
        {
            return new Position(a.X % b.X, a.Y % b.Y, a.Z % b.Z);
        }

        public static Position operator +(Position a, int b)
        {
            return new Position(a.X + b, a.Y + b, a.Z + b);
        }

        public static Position operator -(Position a, int b)
        {
            return new Position(a.X - b, a.Y - b, a.Z - b);
        }

        public static Position operator *(Position a, int b)
        {
            return new Position(a.X * b, a.Y * b, a.Z * b);
        }

        public static Position operator /(Position a, int b)
        {
            return new Position(a.X / b, a.Y / b, a.Z / b);
        }

        public static Position operator %(Position a, int b)
        {
            return new Position(a.X % b, a.Y % b, a.Z % b);
        }

        public static Position operator +(int a, Position b)
        {
            return new Position(a + b.X, a + b.Y, a + b.Z);
        }

        public static Position operator -(int a, Position b)
        {
            return new Position(a - b.X, a - b.Y, a - b.Z);
        }

        public static Position operator *(int a, Position b)
        {
            return new Position(a * b.X, a * b.Y, a * b.Z);
        }

        public static Position operator /(int a, Position b)
        {
            return new Position(a / b.X, a / b.Y, a / b.Z);
        }

        public static Position operator %(int a, Position b)
        {
            return new Position(a % b.X, a % b.Y, a % b.Z);
        }

        public static explicit operator Position(Coordinates2D a)
        {
            return new Position(a.X, 0, a.Z);
        }

        public static implicit operator Position(Vector3 a)
        {
            return new Position((int)a.X, (int)a.Y, (int)a.Z);
        }

        public static implicit operator Vector3(Position a)
        {
            return new Vector3(a.X, a.Y, a.Z);
        }

        #endregion

        #region Constants

        public static readonly Position Zero = new Position(0);
        public static readonly Position One = new Position(1);

        public static readonly Position Up = new Position(0, 1, 0);
        public static readonly Position Down = new Position(0, -1, 0);
        public static readonly Position Left = new Position(-1, 0, 0);
        public static readonly Position Right = new Position(1, 0, 0);
        public static readonly Position Backwards = new Position(0, 0, -1);
        public static readonly Position Forwards = new Position(0, 0, 1);

        public static readonly Position East = new Position(1, 0, 0);
        public static readonly Position West = new Position(-1, 0, 0);
        public static readonly Position North = new Position(0, 0, -1);
        public static readonly Position South = new Position(0, 0, 1);

        #endregion

        public bool Equals(Position other)
        {
            return other.X.Equals(X) && other.Y.Equals(Y) && other.Z.Equals(Z);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Position)) return false;
            return Equals((Position)obj);
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
