using MineLib.Network.IO;
using MineLib.Network.Main.Enums;

namespace MineLib.Network.Main.Packets.Server
{
    public struct ServerDifficultyPacket : IPacket
    {
        public Difficulty Difficulty;

        public byte ID { get { return 0x41; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Difficulty = (Difficulty) reader.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteByte((byte) Difficulty);
            stream.Purge();
        }
    }
}
