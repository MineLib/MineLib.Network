using MineLib.Network.IO;

namespace MineLib.Network.PocketEdition.Packets.Client
{
    public class LoginPacket : IPacketWithSize
    {
        public string Username;
        public int Protocol1;
        public int Protocol2;
        public int ClientID;
        public string RealmsData;

        public byte ID { get { return 0x82; } }
        public short Size { get { return 0; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Username = reader.ReadString();
            Protocol1 = reader.ReadInt();
            Protocol2 = reader.ReadInt();
            ClientID = reader.ReadInt();
            RealmsData = reader.ReadString();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Username);
            stream.WriteInt(Protocol1);
            stream.WriteInt(Protocol2);
            stream.WriteInt(ClientID);
            stream.WriteString(RealmsData);
            stream.Purge();
        }
    }
}
