using MineLib.Network.IO;
using MineLib.Network.Main.Data;
using MineLib.Network.Main.Enums;

namespace MineLib.Network.Main.Packets.Server
{
    public struct PlayerPositionAndLookPacket : IPacket
    {
        public Vector3 Vector3;
        public float Yaw, Pitch;
        public PlayerPositionAndLookFlags Flags;

        public byte ID { get { return 0x08; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Vector3 = Vector3.FromReaderDouble(reader);
            Yaw = reader.ReadFloat();
            Pitch = reader.ReadFloat();
            Flags = (PlayerPositionAndLookFlags) reader.ReadSByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            Vector3.ToStreamDouble(ref stream);
            stream.WriteFloat(Yaw);
            stream.WriteFloat(Pitch);
            stream.WriteSByte((sbyte) Flags);
            stream.Purge();
        }
    }
}