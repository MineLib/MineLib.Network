using CWrapped;
using MineLib.Network.Enums;

namespace MineLib.Network.Packets.Server
{
    public struct HandshakePacket : IPacket
    {
        public int ProtocolVersion;
        public string ServerAddress;
        public short ServerPort;
        public NextState NextState;

        public const byte PacketId = 0x00;
        public byte Id { get { return 0x00; } }

        public void ReadPacket(ref Wrapped stream)
        {
            ProtocolVersion = stream.ReadVarInt();
            ServerAddress = stream.ReadString();
            ServerPort = stream.ReadShort();
            NextState = (NextState)stream.ReadVarInt();
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(ProtocolVersion);
            stream.WriteString(ServerAddress);
            stream.WriteShort(ServerPort);
            stream.WriteVarInt((byte)NextState);
            stream.Purge();
        }
    }

    //


    //

    //

    //

    //
    //
    //
    //

    //
    //
    //

    // TODO//
    // TODO//
    //
    //
    // TODO//
    //
    //

    //
    //

    //
    //
    //
    //
    // TODO
    //
    //

    //

    //public struct UpdateBlockEntityPacket : IPacket
    //{
    //    public int X;
    //    public short Y;
    //    public int Z;
    //    public byte Action;
    //    public NbtFile Nbt;
    //
    //    public const byte PacketId = 0x35;
    //    public byte Id { get { return 0x35; } }
    //
    //    public void ReadPacket(ref Wrapped stream)
    //    {
    //        X = stream.readShort();
    //        Y = stream.readShort();
    //        Z = stream.readShort();
    //        Action = stream.readVarInt();
    //        var length = stream.readShort();
    //        var data = stream.ReadUInt8Array(length);
    //        Nbt = new NbtFile();
    //        Nbt.LoadFromBuffer(data, 0, length, NbtCompression.GZip, null);
    //    }
    //
    //    public void WritePacket(ref Wrapped stream)
    //    {
    //        stream.writeVarInt(Id);
    //        stream.writeVarInt(X);
    //        stream.writeShort(Y);
    //        stream.writeVarInt(Z);
    //        stream.writeVarInt(Action);
    //        var tempStream = new MemoryStream();
    //        Nbt.SaveToStream(tempStream, NbtCompression.GZip);
    //        var buffer = tempStream.GetBuffer();
    //        stream.writeShort((short)buffer.Length);
    //        stream.writeVarIntArray(buffer);
    //    }
    //}

    //public struct StatisticsOpenPacket : IPacket
    //{
    //    public int Count;
    //    public Entry Entry;
    //
    //    public const byte PacketId = 0x37;
    //    public byte Id { get { return 0x37; } }
    //
    //    public void ReadPacket(ref Wrapped stream)
    //    {
    //        Count = stream.readShort();
    //        Entry.StatisticsName = stream.readString();
    //        Entry.Value = stream.readShort();
    //    }
    //
    //    public void WritePacket(ref Wrapped stream)
    //    {
    //        stream.writeVarInt(Id);
    //        stream.writeVarInt(Count);
    //        stream.writeString(Entry.StatisticsName);
    //        stream.writeVarInt(Entry.Value);
    //    }
    //}

    //

    //


    //
    //

    //
    //


    //
    //
    //

    //\\
    //public struct CreativeInventoryActionPacket : IPacket
    //{
    //    public short SlotIndex;
    //    public ItemStack Item;
    //
    //    public const byte PacketId = 0x6B;
    //    public byte Id { get { return 0x6B; } }
    //
    //    public void ReadPacket(ref Wrapped stream)
    //    {
    //        SlotIndex = stream.readShort();
    //        Item = ItemStack.FromStream(stream);
    //    }
    //
    //    public void WritePacket(ref Wrapped stream)
    //    {
    //        stream.writeVarInt(Id);
    //        stream.writeShort(SlotIndex);
    //        Item.WriteTo(stream);
    //    }
    //}
    //

    //

    //

    //\\
    //public struct ClientSettingsPacket : IPacket
    //{
    //    public string Locale;
    //    public byte ViewDistance;
    //    public ChatMode ChatFlags;
    //    public Difficulty Difficulty;
    //    public bool ShowCape;
    //
    //    public const byte PacketId = 0xCC;
    //    public byte Id { get { return 0xCC; } }
    //
    //    public void ReadPacket(ref Wrapped stream)
    //    {
    //        Locale = stream.readString();
    //        ViewDistance = stream.readVarInt();
    //        ChatFlags = (ChatMode)(stream.ReadUInt8() & 0x3);
    //        Difficulty = (Difficulty)stream.readVarInt();
    //        ShowCape = stream.readBool();
    //    }
    //
    //    public void WritePacket(ref Wrapped stream)
    //    {
    //        stream.writeVarInt(Id);
    //        stream.writeString(Locale);
    //        stream.writeVarInt(ViewDistance);
    //        stream.writeVarInt((byte)ChatFlags);
    //        stream.writeVarInt((byte)Difficulty);
    //        stream.writeBool(ShowCape);
    //    }
    //}

    //
    //
}