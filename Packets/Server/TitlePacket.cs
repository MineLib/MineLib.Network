using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public interface ITitleAction
    {
        ITitleAction FromReader(PacketByteReader reader);
        void ToStream(ref PacketStream stream);
    }

    public struct TitleTitle : ITitleAction
    {
        public string Text;

        public ITitleAction FromReader(PacketByteReader reader)
        {
            Text = reader.ReadString();

            return this; // Hope works
        }

        public void ToStream(ref PacketStream stream)
        {
            stream.WriteString(Text);
        }
    }

    public struct TitleSubtitle : ITitleAction
    {
        public string Text;

        public ITitleAction FromReader(PacketByteReader reader)
        {
            Text = reader.ReadString();

            return this; // Hope works
        }

        public void ToStream(ref PacketStream stream)
        {
            stream.WriteString(Text);
        }
    }

    public struct TitleTimes : ITitleAction
    {
        public int FadeIn;
        public int Stay;
        public int FadeOut;

        public ITitleAction FromReader(PacketByteReader reader)
        {
            FadeIn = reader.ReadInt();
            Stay = reader.ReadInt();
            FadeOut = reader.ReadInt();

            return this; // Hope works
        }

        public void ToStream(ref PacketStream stream)
        {
            stream.WriteInt(FadeIn);
            stream.WriteInt(Stay);
            stream.WriteInt(FadeOut);
        }
    }

    public struct TitleClear : ITitleAction
    {
        public ITitleAction FromReader(PacketByteReader reader)
        {
            return this; // Hope works
        }

        public void ToStream(ref PacketStream stream)
        {
        }
    }

    public struct TitleReset : ITitleAction
    {
        public ITitleAction FromReader(PacketByteReader reader)
        {
            return this; // Hope works
        }

        public void ToStream(ref PacketStream stream)
        {
        }
    }

    public struct TitlePacket : IPacket
    {
        public int Action;
        public ITitleAction TitleAction;

        public const byte PacketID = 0x45;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Action = reader.ReadVarInt();

            switch (Action)
            {
                case 0:
                    TitleAction = new TitleTitle();
                    TitleAction.FromReader(reader);
                    break;
                case 1:
                    TitleAction = new TitleSubtitle();
                    TitleAction.FromReader(reader);
                    break;
                case 2:
                    TitleAction = new TitleTimes();
                    TitleAction.FromReader(reader);
                    break;
                case 3:
                    TitleAction = new TitleClear();
                    TitleAction.FromReader(reader);
                    break;
                case 4:
                    TitleAction = new TitleReset();
                    TitleAction.FromReader(reader);
                    break;
            }
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(Action);
            TitleAction.ToStream(ref  stream);
            stream.Purge();
        }
    }
}
