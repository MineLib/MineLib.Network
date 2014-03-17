using MineLib.Network.IO;
using MineLib.Network.Enums;


namespace MineLib.Network.Packets.Server
{
    public class TeamsPacket : IPacket
    {
        public string TeamName;
        public TeamMode Mode;
        public string TeamDisplayName;
        public string TeamPrefix;
        public string TeamSuffix;
        public byte FriendlyFire;
        public short PlayerCount;
        public string[] Players;

        public const byte PacketID = 0x3E;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(ref Wrapped stream)
        {
            TeamName = stream.ReadString();
            Mode = (TeamMode)stream.ReadByte();

            if (Mode == TeamMode.UpdateTeam || Mode == TeamMode.CreateTeam)
            {
                TeamDisplayName = stream.ReadString();
                TeamPrefix = stream.ReadString();
                TeamSuffix = stream.ReadString();
                FriendlyFire = stream.ReadByte();
            }

            if (Mode == TeamMode.CreateTeam || Mode == TeamMode.AddPlayers || Mode == TeamMode.RemovePlayers)
            {
                PlayerCount = stream.ReadShort();

                Players = new string[PlayerCount];
                for (int i = 0; i < PlayerCount; i++)
                {
                    Players[i] = stream.ReadString();
                }
            }
        }

        public void WritePacket(ref Wrapped stream)
        {
            stream.WriteVarInt(Id);

            stream.WriteString(TeamName);
            stream.WriteByte((byte)Mode);
            if (Mode == TeamMode.UpdateTeam || Mode == TeamMode.CreateTeam)
            {
                stream.WriteString(TeamDisplayName);
                stream.WriteString(TeamPrefix);
                stream.WriteString(TeamSuffix);
                stream.WriteByte(FriendlyFire);
            }

            if (Mode == TeamMode.CreateTeam || Mode == TeamMode.AddPlayers || Mode == TeamMode.RemoveTeam)
            {
                stream.WriteShort((short)Players.Length);
                stream.WriteStringArray(Players);
            }
        }
    }
}
