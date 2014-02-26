namespace MineLib.Network.Data
{
    public struct EntityPropertyListItem1
    {
        public long UUID;
        public double Amount;
        public byte Operation;
    }

    public struct EntityPropertyListItem
    {
        public long UnknownMSB, UnknownLSB;
        public double UnknownDouble;
        public byte UnknownByte;
    }
}
