using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public interface IWorldBorder
    {
        IWorldBorder FromReader(PacketByteReader reader);
        void ToStream(ref PacketStream stream);
    }

    public struct WorldBorderSetSize : IWorldBorder
    {
        public double Radius;
        
        public IWorldBorder FromReader(PacketByteReader reader)
        {
            Radius = reader.ReadDouble();

            return this; // Hope works
        }

        public void ToStream(ref PacketStream stream)
        {
            stream.WriteDouble(Radius);
        }
    }

    public struct WorldBorderLerpSize : IWorldBorder
    {
        public double OldRadius;
        public double NewRadius;
        public long Speed;

        public IWorldBorder FromReader(PacketByteReader reader)
        {
            OldRadius = reader.ReadDouble();
            NewRadius = reader.ReadDouble();
            //Speed = stream.ReadVarLong(); TODO: VarLong

            return this; // Hope works
        }

        public void ToStream(ref PacketStream stream)
        {
            stream.WriteDouble(OldRadius);
            stream.WriteDouble(NewRadius);
            //stream.WriteVarLong(Speed); TODO: VarLong
        }
    }

    public struct WorldBorderSetCenter : IWorldBorder
    {
        public double X, Z;

        public IWorldBorder FromReader(PacketByteReader reader)
        {
            X = reader.ReadDouble();
            Z = reader.ReadDouble();

            return this; // Hope works
        }

        public void ToStream(ref PacketStream stream)
        {
            stream.WriteDouble(X);
            stream.WriteDouble(Z);
        }
    }

    public struct WorldBorderInitialize : IWorldBorder
    {
        public double X, Z;
        public double OldRadius;
        public double NewRadius;
        public long Speed;
        public int PortalTeleportBoundary;
        public int WarningTime;
        public int WarningBlocks;

        public IWorldBorder FromReader(PacketByteReader reader)
        {
            X = reader.ReadDouble();
            Z = reader.ReadDouble();

            OldRadius = reader.ReadDouble();
            NewRadius = reader.ReadDouble();
            //Speed = stream.ReadVarLong(); TODO: VarLong
            PortalTeleportBoundary = reader.ReadVarInt();
            WarningTime = reader.ReadVarInt();
            WarningBlocks = reader.ReadVarInt();

            return this; // Hope works
        }

        public void ToStream(ref PacketStream stream)
        {
            stream.WriteDouble(X);
            stream.WriteDouble(Z);

            stream.WriteDouble(OldRadius);
            stream.WriteDouble(NewRadius);
            //stream.WriteVarLong(Speed); TODO: VarLong
            stream.WriteVarInt(PortalTeleportBoundary);
            stream.WriteVarInt(WarningTime);
            stream.WriteVarInt(WarningBlocks);
        }
    }

    public struct WorldBorderSetWarningTime : IWorldBorder
    {
        public int WarningTime;

        public IWorldBorder FromReader(PacketByteReader reader)
        {
            WarningTime = reader.ReadVarInt();

            return this; // Hope works
        }

        public void ToStream(ref PacketStream stream)
        {
            stream.WriteVarInt(WarningTime);
        }
    }

    public struct WorldBorderSetWarningBlocks : IWorldBorder
    {
        public int WarningBlocks;

        public IWorldBorder FromReader(PacketByteReader reader)
        {
            WarningBlocks = reader.ReadVarInt();

            return this; // Hope works
        }

        public void ToStream(ref PacketStream stream)
        {
            stream.WriteVarInt(WarningBlocks);
        }
    }

    public struct WorldBorderPacket : IPacket
    {
        public int Action;

        public IWorldBorder WorldBorderAction;

        public const byte PacketID = 0x44;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Action = reader.ReadVarInt();

            switch (Action)
            {
                case 0:
                    WorldBorderAction = new WorldBorderSetSize().FromReader(reader);
                    break;
                case 1:
                    WorldBorderAction = new WorldBorderLerpSize().FromReader(reader);
                    break;
                case 2:
                    WorldBorderAction = new WorldBorderSetCenter().FromReader(reader);
                    break;
                case 3:
                    WorldBorderAction = new WorldBorderInitialize().FromReader(reader);
                    break;
                case 4:
                    WorldBorderAction = new WorldBorderSetWarningTime().FromReader(reader);
                    break;
                case 5:
                    WorldBorderAction = new WorldBorderSetWarningBlocks().FromReader(reader);
                    break;
            }
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteVarInt(Action);
            WorldBorderAction.ToStream(ref stream);
            stream.Purge();
        }
    }
}
