using MineLib.Network.IO;
using MineLib.Network.Enums;

namespace MineLib.Network.Packets.Server
{
    // TODO: Shit
    public class TeamsPacket : IPacket
    {
        public string TeamName;
        public TeamMode Mode;
        public string TeamDisplayName;
        public string TeamPrefix;
        public string TeamSuffix;
        public byte FriendlyFire;
        public string NameTagVisibility;
        public byte Color;
        public short PlayerCount;
        public string[] Players;

        public const byte PacketID = 0x3E;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            TeamName = reader.ReadString();
            Mode = (TeamMode) reader.ReadByte();

            if (Mode == TeamMode.CreateTeam || Mode == TeamMode.UpdateTeam)
            {
                TeamDisplayName = reader.ReadString();
                TeamPrefix = reader.ReadString();
                TeamSuffix = reader.ReadString();
                FriendlyFire = reader.ReadByte();
                NameTagVisibility = reader.ReadString();
                Color = reader.ReadByte();
            }

            if (Mode == TeamMode.CreateTeam || Mode == TeamMode.AddPlayers || Mode == TeamMode.RemovePlayers)
            {
                PlayerCount = reader.ReadShort();

                Players = new string[PlayerCount];
                for (int i = 0; i < PlayerCount; i++)
                {
                    Players[i] = reader.ReadString();
                }
            }
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);

            stream.WriteString(TeamName);
            stream.WriteByte((byte) Mode);

            if (Mode == TeamMode.CreateTeam || Mode == TeamMode.UpdateTeam)
            {
                stream.WriteString(TeamDisplayName);
                stream.WriteString(TeamPrefix);
                stream.WriteString(TeamSuffix);
                stream.WriteByte(FriendlyFire);
                stream.WriteString(NameTagVisibility);
                stream.WriteByte(Color);
            }

            if (Mode == TeamMode.CreateTeam || Mode == TeamMode.AddPlayers || Mode == TeamMode.RemoveTeam)
            {
                stream.WriteShort((short) Players.Length);
                stream.WriteStringArray(Players);
            }
        }
    }
}
