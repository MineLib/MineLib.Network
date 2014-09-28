using MineLib.Network.IO;
using MineLib.Network.Enums;

namespace MineLib.Network.Packets.Server
{
    public struct ChangeGameStatePacket : IPacket
    {
        public GameStateReason Reason;
        public float Value;

        public byte ID { get { return 0x2B; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Reason = (GameStateReason) reader.ReadByte();
            Value = reader.ReadFloat();
        }
    
        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteByte((byte) Reason);
            stream.WriteFloat(Value);
            stream.Purge();
        }
    }
}