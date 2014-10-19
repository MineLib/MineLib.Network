using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public interface ICombatEvent
    {
        ICombatEvent FromReader(MinecraftDataReader reader);
        void ToStream(ref MinecraftStream stream);
    }

    public struct CombatEventEnterCombat : ICombatEvent
    {
        public ICombatEvent FromReader(MinecraftDataReader reader)
        {
            return this; // Hope works TODO: Check this
        }

        public void ToStream(ref MinecraftStream stream)
        {
        }
    }

    public struct CombatEventEndCombat : ICombatEvent
    {
        public int Duration;
        public int EntityID;

        public ICombatEvent FromReader(MinecraftDataReader reader)
        {
            Duration = reader.ReadVarInt();
            EntityID = reader.ReadInt();

            return this; // Hope works
        }

        public void ToStream(ref MinecraftStream stream)
        {
            stream.WriteVarInt(Duration);
            stream.WriteInt(EntityID);
        }
    }

    public struct CombatEventEntityDead : ICombatEvent
    {
        public int PlayerID;
        public int EntityID;
        public string Message;

        public ICombatEvent FromReader(MinecraftDataReader reader)
        {
            PlayerID = reader.ReadVarInt();
            EntityID = reader.ReadInt();
            Message = reader.ReadString();

            return this; // Hope works
        }

        public void ToStream(ref MinecraftStream stream)
        {
            stream.WriteVarInt(PlayerID);
            stream.WriteInt(EntityID);
            stream.WriteString(Message);
        }
    }

    public struct CombatEventPacket : IPacket
    {
        public int Event;

        public ICombatEvent CombatEvent;

        public byte ID { get { return 0x42; } }

        public IPacket ReadPacket(MinecraftDataReader reader)
        {
            Event = reader.ReadVarInt();

            switch (Event)
            {
                case 0:
                    CombatEvent = new CombatEventEnterCombat().FromReader(reader);
                    break;

                case 1:
                    CombatEvent = new CombatEventEndCombat().FromReader(reader);
                    break;

                case 2:
                    CombatEvent = new CombatEventEntityDead().FromReader(reader);
                    break;
            }


            return this;
        }

        public IPacket WritePacket(MinecraftStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(Event);
            CombatEvent.ToStream(ref stream);
            stream.Purge();

            return this;
        }
    }
}
