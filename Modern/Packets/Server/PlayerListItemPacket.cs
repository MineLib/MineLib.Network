using System.Collections.Generic;
using MineLib.Network.IO;
using MineLib.Network.Modern.Enums;
using Org.BouncyCastle.Math;

namespace MineLib.Network.Modern.Packets.Server
{
    public interface IPlayerList
    {
        IPlayerList FromReader(IMinecraftDataReader reader);
        void ToStream(ref IMinecraftStream stream);
    }

    public struct Properties
    {
        public string Name;
        public string Value;
        public bool IsSigned;
        public string Signature;
    }

    public class PlayerListActionProperties
    {
        private readonly List<Properties> _entries;

        public PlayerListActionProperties()
        {
            _entries = new List<Properties>();
        }

        public int Count
        {
            get { return _entries.Count; }
        }

        public Properties this[int index]
        {
            get { return _entries[index]; }
            set { _entries.Insert(index, value); }
        }

        public static PlayerListActionProperties FromReader(IMinecraftDataReader reader)
        {
            var count = reader.ReadVarInt();

            var value = new PlayerListActionProperties();
            for (var i = 0; i < count; i++)
            {
                var property = new Properties();

                property.Name = reader.ReadString();
                property.Value = reader.ReadString();
                property.IsSigned = reader.ReadBoolean();

                if (property.IsSigned)
                    property.Signature = reader.ReadString();

                value[i] = property;
            }

            return value;
        }

        public void ToStream(ref IMinecraftStream stream)
        {
            stream.WriteVarInt(Count);

            foreach (var entry in _entries)
            {
                stream.WriteString(entry.Name);
                stream.WriteString(entry.Value);
                stream.WriteBoolean(entry.IsSigned);
                if (entry.IsSigned)
                    stream.WriteString(entry.Signature);
            }
        }
    }

    public struct PlayerListActionAddPlayer : IPlayerList
    {
        public string Name;
        public PlayerListActionProperties Properties;
        public int Gamemode;
        public int Ping;
        public bool HasDisplayName;
        public string DisplayName;

        public IPlayerList FromReader(IMinecraftDataReader reader)
        {
            Name = reader.ReadString();
            Properties = PlayerListActionProperties.FromReader(reader);

            Gamemode = reader.ReadVarInt();
            Ping = reader.ReadVarInt();
            HasDisplayName = reader.ReadBoolean();
            if (HasDisplayName)
                DisplayName = reader.ReadString();

            return this;
        }

        public void ToStream(ref IMinecraftStream stream)
        {
            stream.WriteString(Name);
            Properties.ToStream(ref stream);
            stream.WriteVarInt(Gamemode);
            stream.WriteVarInt(Ping);
            stream.WriteBoolean(HasDisplayName);
            if (HasDisplayName)
                stream.WriteString(DisplayName);
            
        }
    }

    public struct PlayerListActionUpdateGamemode : IPlayerList
    {
        public int Gamemode;

        public IPlayerList FromReader(IMinecraftDataReader reader)
        {
            Gamemode = reader.ReadVarInt();
            
            return this;
        }

        public void ToStream(ref IMinecraftStream stream)
        {
            stream.WriteVarInt(Gamemode);
        }
    }

    public struct PlayerListActionUpdateLatency : IPlayerList
    {
        public int Ping;

        public IPlayerList FromReader(IMinecraftDataReader reader)
        {
            Ping = reader.ReadVarInt();

            return this;
        }

        public void ToStream(ref IMinecraftStream stream)
        {
            stream.WriteVarInt(Ping);
        }
    }

    public struct PlayerListActionUpdateDisplayName : IPlayerList
    {
        public bool HasDisplayName;
        public string DisplayName;

        public IPlayerList FromReader(IMinecraftDataReader reader)
        {
            HasDisplayName = reader.ReadBoolean();
            DisplayName = reader.ReadString();

            return this;
        }

        public void ToStream(ref IMinecraftStream stream)
        {
            stream.WriteBoolean(HasDisplayName);
            stream.WriteString(DisplayName);
        }
    }

    public struct PlayerListActionRemovePlayer : IPlayerList
    {
        public IPlayerList FromReader(IMinecraftDataReader reader)
        {
            return this;
        }

        public void ToStream(ref IMinecraftStream stream)
        {
        }
    }

    public struct PlayerListItemPacket : IPacket
    {
        public PlayerListAction Action;
        public int Length;
        public BigInteger UUID;
        public IPlayerList PlayerList;

        public byte ID { get { return 0x38; } }

        public IPacket ReadPacket(IMinecraftDataReader reader)
        {
            Action = (PlayerListAction) reader.ReadVarInt();
            Length = reader.ReadVarInt();
            UUID = reader.ReadBigInteger();

            switch (Action)
            {
                case PlayerListAction.AddPlayer:
                    PlayerList = new PlayerListActionAddPlayer().FromReader(reader);
                    break;
                case PlayerListAction.UpdateGamemode:
                    PlayerList = new PlayerListActionUpdateGamemode().FromReader(reader);
                    break;
                case PlayerListAction.UpdateLatency:
                    PlayerList = new PlayerListActionUpdateLatency().FromReader(reader);
                    break;
                case PlayerListAction.UpdateDisplayName:
                    PlayerList = new PlayerListActionUpdateDisplayName().FromReader(reader);
                    break;
                case PlayerListAction.RemovePlayer:
                    PlayerList = new PlayerListActionRemovePlayer().FromReader(reader);
                    break;
            }

            return this;
        }

        public IPacket WritePacket(IMinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt((byte) Action);
            stream.WriteVarInt(Length);
            stream.WriteBigInteger(UUID);
            PlayerList.ToStream(ref stream);
            stream.Purge();

            return this;
        }
    }
}