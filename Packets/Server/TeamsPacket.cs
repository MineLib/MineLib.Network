using MineLib.Network.IO;
using MineLib.Network.Enums;

namespace MineLib.Network.Packets.Server
{
    public interface ITeam
    {
        ITeam FromReader(PacketByteReader reader);
        void ToStream(ref PacketStream stream);
    }

    public struct TeamsCreateTeam : ITeam
    {
        public string TeamDisplayName;
        public string TeamPrefix;
        public string TeamSuffix;
        public byte FriendlyFire;
        public string NameTagVisibility;
        public byte Color;
        public string[] Players;

        public ITeam FromReader(PacketByteReader reader)
        {
            TeamDisplayName = reader.ReadString();
            TeamPrefix = reader.ReadString();
            TeamSuffix = reader.ReadString();
            FriendlyFire = reader.ReadByte();
            NameTagVisibility = reader.ReadString();
            Color = reader.ReadByte();

            var count = reader.ReadVarInt();
            Players = new string[count];
            for (var i = 0; i < count; i++)
            {
                Players[i] = reader.ReadString();
            }

            return this;
        }

        public void ToStream(ref PacketStream stream)
        {
            stream.WriteString(TeamDisplayName);
            stream.WriteString(TeamPrefix);
            stream.WriteString(TeamSuffix);
            stream.WriteByte(FriendlyFire);
            stream.WriteString(NameTagVisibility);
            stream.WriteByte(Color);
            stream.WriteVarInt(Players.Length);
            stream.WriteStringArray(Players);
        }
    }

    public struct TeamsRemoveTeam : ITeam
    {
        public ITeam FromReader(PacketByteReader reader)
        {
            return this;
        }

        public void ToStream(ref PacketStream stream)
        {
        }
    }

    public struct TeamsUpdateTeam : ITeam
    {
        public string TeamDisplayName;
        public string TeamPrefix;
        public string TeamSuffix;
        public byte FriendlyFire;
        public string NameTagVisibility;
        public byte Color;

        public ITeam FromReader(PacketByteReader reader)
        {
            TeamDisplayName = reader.ReadString();
            TeamPrefix = reader.ReadString();
            TeamSuffix = reader.ReadString();
            FriendlyFire = reader.ReadByte();
            NameTagVisibility = reader.ReadString();
            Color = reader.ReadByte();

            return this;
        }

        public void ToStream(ref PacketStream stream)
        {
            stream.WriteString(TeamDisplayName);
            stream.WriteString(TeamPrefix);
            stream.WriteString(TeamSuffix);
            stream.WriteByte(FriendlyFire);
            stream.WriteString(NameTagVisibility);
            stream.WriteByte(Color);
        }
    }

    public struct TeamsAddPlayers : ITeam
    {
        public string[] Players;

        public ITeam FromReader(PacketByteReader reader)
        {
            var count = reader.ReadVarInt();
            Players = new string[count];
            for (var i = 0; i < count; i++)
            {
                Players[i] = reader.ReadString();
            }

            return this;
        }

        public void ToStream(ref PacketStream stream)
        {
            stream.WriteVarInt(Players.Length);
            stream.WriteStringArray(Players);
        }
    }

    public struct TeamsRemovePlayers : ITeam
    {
        public string[] Players;

        public ITeam FromReader(PacketByteReader reader)
        {
            var count = reader.ReadVarInt();
            Players = new string[count];
            for (var i = 0; i < count; i++)
            {
                Players[i] = reader.ReadString();
            }

            return this;
        }

        public void ToStream(ref PacketStream stream)
        {
            stream.WriteVarInt(Players.Length);
            stream.WriteStringArray(Players);
        }
    }

    public class TeamsPacket : IPacket
    {
        public string TeamName;
        public TeamAction Action;
        public ITeam Team;

        public const byte PacketID = 0x3E;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            TeamName = reader.ReadString();
            Action = (TeamAction) reader.ReadByte();

            switch (Action)
            {
                case TeamAction.CreateTeam:
                    Team = new TeamsCreateTeam().FromReader(reader);
                    break;
                case TeamAction.RemoveTeam:
                    Team = new TeamsRemoveTeam().FromReader(reader);
                    break;
                case TeamAction.UpdateTeam:
                    Team = new TeamsUpdateTeam().FromReader(reader);
                    break;
                case TeamAction.AddPlayers:
                    Team = new TeamsAddPlayers().FromReader(reader);
                    break;
                case TeamAction.RemovePlayers:
                    Team = new TeamsRemovePlayers().FromReader(reader);
                    break;
            }
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(TeamName);
            stream.WriteByte((byte) Action);
            Team.ToStream(ref stream);
            stream.Purge();
        }
    }
}
