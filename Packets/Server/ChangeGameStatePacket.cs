using MineLib.Network.IO;
using MineLib.Network.Enums;

namespace MineLib.Network.Packets.Server
{
    public struct ChangeGameStatePacket : IPacket
    {
        public GameStateReason Reason;
        public float Value;

        public const byte PacketID = 0x2B;
        public byte Id { get { return PacketID; } }
    
        public void ReadPacket(PacketByteReader stream)
        {
            Reason = (GameStateReason)stream.ReadByte();
            Value = stream.ReadFloat();
        }
    
        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteByte((byte)Reason);
            stream.WriteFloat(Value);
            stream.Purge();
        }
    }
}