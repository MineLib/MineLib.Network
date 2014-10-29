using MineLib.Network.IO;
using MineLib.Network.Modern.Enums;

namespace MineLib.Network.Modern.Packets.Server
{
    public interface ITitle
    {
        ITitle FromReader(IMinecraftDataReader reader);
        void ToStream(ref IMinecraftStream stream);
    }

    public struct TitleTitle : ITitle
    {
        public string Text;

        public ITitle FromReader(IMinecraftDataReader reader)
        {
            Text = reader.ReadString();

            return this;
        }

        public void ToStream(ref IMinecraftStream stream)
        {
            stream.WriteString(Text);
        }
    }

    public struct TitleSubtitle : ITitle
    {
        public string Text;

        public ITitle FromReader(IMinecraftDataReader reader)
        {
            Text = reader.ReadString();

            return this;
        }

        public void ToStream(ref IMinecraftStream stream)
        {
            stream.WriteString(Text);
        }
    }

    public struct TitleTimes : ITitle
    {
        public int FadeIn;
        public int Stay;
        public int FadeOut;

        public ITitle FromReader(IMinecraftDataReader reader)
        {
            FadeIn = reader.ReadInt();
            Stay = reader.ReadInt();
            FadeOut = reader.ReadInt();

            return this;
        }

        public void ToStream(ref IMinecraftStream stream)
        {
            stream.WriteInt(FadeIn);
            stream.WriteInt(Stay);
            stream.WriteInt(FadeOut);
        }
    }

    public struct TitleClear : ITitle
    {
        public ITitle FromReader(IMinecraftDataReader reader)
        {
            return this;
        }

        public void ToStream(ref IMinecraftStream stream)
        {
        }
    }

    public struct TitleReset : ITitle
    {
        public ITitle FromReader(IMinecraftDataReader reader)
        {
            return this;
        }

        public void ToStream(ref IMinecraftStream stream)
        {
        }
    }

    public struct TitlePacket : IPacket
    {
        public TitleAction Action;
        public ITitle Title;

        public byte ID { get { return 0x45; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
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

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt((byte) Action);
            Title.ToStream(ref  stream);
            stream.Purge();

            return this;
        }
    }
}
