using MineLib.Network.IO;

namespace MineLib.Network.Modern.Packets.Server
{
    public interface ICombatEvent
    {
        ICombatEvent FromReader(PacketByteReader reader);
        void ToStream(ref PacketStream stream);
    }

    public struct CombatEventEnterCombat : ICombatEvent
    {
        public ICombatEvent FromReader(PacketByteReader reader)
        {
            return this; // Hope works TODO: Check this
        }

        public void ToStream(ref PacketStream stream)
        {
        }
    }

    public struct CombatEventEndCombat : ICombatEvent
    {
        public int Duration;
        public int EntityID;

        public ICombatEvent FromReader(PacketByteReader reader)
        {
            Duration = reader.ReadVarInt();
            EntityID = reader.ReadInt();

            return this; // Hope works
        }

        public void ToStream(ref PacketStream stream)
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

        public ICombatEvent FromReader(PacketByteReader reader)
        {
            PlayerID = reader.ReadVarInt();
            EntityID = reader.ReadInt();
            Message = reader.ReadString();

            return this; // Hope works
        }

        public void ToStream(ref PacketStream stream)
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

        public void ReadPacket(PacketByteReader reader)
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

        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(Event);
            CombatEvent.ToStream(ref stream);
            stream.Purge();
        }
    }
}
