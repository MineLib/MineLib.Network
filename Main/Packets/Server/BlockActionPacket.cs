using MineLib.Network.IO;
using MineLib.Network.Main.Data;
using MineLib.Network.Main.Enums;

namespace MineLib.Network.Main.Packets.Server
{
    public interface IBlockAction
    {
        IBlockAction FromReader(PacketByteReader reader); // --- Is not used
        void ToStream(ref PacketStream stream);
    }

    public struct BlockActionNoteBlock : IBlockAction
    {
        public int BlockType;

        public NoteBlockType NoteBlockType;
        public int Pitch;

        public BlockActionNoteBlock(byte byte1, byte byte2, int blockType)
        {
            NoteBlockType = (NoteBlockType) byte1;
            Pitch = byte2;
            BlockType = blockType;
        }

        public IBlockAction FromReader(PacketByteReader reader)
        {
            return null;
        }

        public void ToStream(ref PacketStream stream)
        {
            stream.WriteByte((byte) NoteBlockType);
            stream.WriteByte((byte) Pitch);
            stream.WriteVarInt(BlockType);
        }
    }

    public struct BlockActionPiston : IBlockAction
    {
        public int BlockType;

        public PistonState PistonState;
        public PistonDirection PistonDirection;

        public BlockActionPiston(byte byte1, byte byte2, int blockType)
        {
            PistonState = (PistonState) byte1;
            PistonDirection = (PistonDirection) byte2;
            BlockType = blockType;
        }

        public IBlockAction FromReader(PacketByteReader reader)
        {
            return null;
        }

        public void ToStream(ref PacketStream stream)
        {
            stream.WriteByte((byte) PistonState);
            stream.WriteByte((byte) PistonDirection);
            stream.WriteVarInt(BlockType);
        }
    }

    public struct BlockActionChest : IBlockAction
    {
        public int BlockType;

        public byte Byte1;
        public ChestState ChestState;

        public BlockActionChest(byte byte1, byte byte2, int blockType)
        {
            Byte1 = 1; // Not used - always 1.
            ChestState = (ChestState) byte2;
            BlockType = blockType;
        }

        public IBlockAction FromReader(PacketByteReader reader)
        {
            return null;
        }

        public void ToStream(ref PacketStream stream)
        {
            stream.WriteByte(Byte1);
            stream.WriteByte((byte) ChestState);
            stream.WriteVarInt(BlockType);
        }
    }

    public struct BlockActionPacket : IPacket
    {
        public Position Location;
        public byte Byte1;
        public byte Byte2;
        public int BlockType;

        public IBlockAction BlockAction;

        public byte ID { get { return 0x24; } }

        public void ReadPacket(PacketByteReader reader)
        {
            Location = Position.FromReaderLong(reader);
            Byte1 = reader.ReadByte();
            Byte2 = reader.ReadByte();
            BlockType = reader.ReadVarInt();

            switch (BlockType)
            {
                case 25:
                    BlockAction = new BlockActionNoteBlock(Byte1, Byte2, BlockType);
                    break;

                case 29:
                case 33:
                    BlockAction = new BlockActionPiston(Byte1, Byte2, BlockType);
                    break;

                case 54:
                case 130: // TODO: Check
                case 146: // TODO: Check
                    BlockAction = new BlockActionChest(Byte1, Byte2, BlockType);
                    break;
            }

        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(ID);
            Location.ToStreamLong(ref stream);
            BlockAction.ToStream(ref stream);
            stream.Purge();
        }
    }
}