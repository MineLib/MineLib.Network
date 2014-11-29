using System;
using MineLib.Network.IO;

namespace MineLib.Network.Data
{
    /// <summary>
    /// Represents mostly head location of an entity
    /// </summary>
    public struct Rotation : IEquatable<Rotation>
    {
        public readonly float Pitch;
        public readonly float Yaw;
        public readonly float Roll;

        public Rotation(float pitch, float yaw, float roll)
        {
            Pitch = pitch;
            Yaw = yaw;
            Roll = roll;
        }

        public Rotation(Rotation v)
        {
            Pitch = v.Pitch;
            Yaw = v.Yaw;
            Roll = v.Roll;
        }


        #region Network

        public Rotation FromReaderFloat(IMinecraftDataReader reader)
        {
            return new Rotation(
                reader.ReadFloat(), 
                reader.ReadFloat(), 
                reader.ReadFloat()
            );
        }


        public void ToStreamFloat(IMinecraftStream stream)
        {
            stream.WriteFloat(Pitch);
            stream.WriteFloat(Yaw);
            stream.WriteFloat(Roll);
        }

        #endregion


        /// <summary>
        /// Converts this Rotation to a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Pitch: {0}, Yaw: {1}, Roll: {2}", Pitch, Yaw, Roll);
        }

        public static bool operator ==(Rotation a, Rotation b)
        {
            return a.Pitch == b.Pitch && a.Yaw == b.Yaw && a.Roll == b.Roll;
        }

        public static bool operator !=(Rotation a, Rotation b)
        {
            return a.Pitch != b.Pitch && a.Yaw != b.Yaw && a.Roll != b.Roll;
        }

        public bool Equals(Rotation other)
        {
            return other.Pitch.Equals(Pitch) && other.Yaw.Equals(Yaw) && other.Roll.Equals(Roll);
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Rotation))
                return false;

            return Equals((Rotation)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var result = Pitch.GetHashCode();
                result = (result * 397) ^ Yaw.GetHashCode();
                result = (result * 397) ^ Roll.GetHashCode();
                return result;
            }
        }
    }
}
