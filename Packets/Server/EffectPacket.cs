using MineLib.Network.IO;


namespace MineLib.Network.Packets.Server
{
    public struct EffectPacket : IPacket
    {
        public int EffectID;
        public int X;
        public byte Y;
        public int Z;
        public int Data;
        public bool DisableRelativeVolume;

        public const byte PacketID = 0x28;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            EffectID = stream.ReadInt();
            X = stream.ReadInt();
            Y = stream.ReadByte();
            Z = stream.ReadInt();
            Data = stream.ReadInt();
            DisableRelativeVolume = stream.ReadBool();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EffectID);
            stream.WriteInt(X);
            stream.WriteByte(Y);
            stream.WriteInt(Z);
            stream.WriteInt(Data);
            stream.WriteBool(DisableRelativeVolume);
            stream.Purge();
        }
    }
}