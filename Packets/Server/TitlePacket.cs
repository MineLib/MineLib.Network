using MineLib.Network.Enums;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public interface ITitle
    {
        ITitle FromReader(PacketByteReader reader);
        void ToStream(ref PacketStream stream);
    }

    public struct TitleTitle : ITitle
    {
        public string Text;

        public ITitle FromReader(PacketByteReader reader)
        {
            Text = reader.ReadString();

            return this;
        }

        public void ToStream(ref PacketStream stream)
        {
            stream.WriteString(Text);
        }
    }

    public struct TitleSubtitle : ITitle
    {
        public string Text;

        public ITitle FromReader(PacketByteReader reader)
        {
            Text = reader.ReadString();

            return this;
        }

        public void ToStream(ref PacketStream stream)
        {
            stream.WriteString(Text);
        }
    }

    public struct TitleTimes : ITitle
    {
        public int FadeIn;
        public int Stay;
        public int FadeOut;

        public ITitle FromReader(PacketByteReader reader)
        {
            FadeIn = reader.ReadInt();
            Stay = reader.ReadInt();
            FadeOut = reader.ReadInt();

            return this;
        }

        public void ToStream(ref PacketStream stream)
        {
            stream.WriteInt(FadeIn);
            stream.WriteInt(Stay);
            stream.WriteInt(FadeOut);
        }
    }

    public struct TitleClear : ITitle
    {
        public ITitle FromReader(PacketByteReader reader)
        {
            return this;
        }

        public void ToStream(ref PacketStream stream)
        {
        }
    }

    public struct TitleReset : ITitle
    {
        public ITitle FromReader(PacketByteReader reader)
        {
            return this;
        }

        public void ToStream(ref PacketStream stream)
        {
        }
    }

    public struct TitlePacket : IPacket
    {
        public TitleAction Action;
        public ITitle Title;

        public byte ID { get { return 0x45; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Action = (TitleAction) reader.ReadVarInt();

            switch (Action)
            {
                case TitleAction.Title:
                    Title = new TitleTitle();
                    Title.FromReader(reader);
                    break;
                case TitleAction.Subtitle:
                    Title = new TitleSubtitle();
                    Title.FromReader(reader);
                    break;
                case TitleAction.Times:
                    Title = new TitleTimes();
                    Title.FromReader(reader);
                    break;
                case TitleAction.Clear:
                    Title = new TitleClear();
                    Title.FromReader(reader);
                    break;
                case TitleAction.Reset:
                    Title = new TitleReset();
                    Title.FromReader(reader);
                    break;
            }
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt((byte) Action);
            Title.ToStream(ref  stream);
            stream.Purge();
        }
    }
}
