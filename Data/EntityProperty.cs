namespace MineLib.Network.Data
{
    public struct EntityProperty
    {
        public EntityProperty(string key, double value)
        {
            Key = key;
            Value = value;
            UnknownList = new EntityPropertyListItem[0];
        }

        public string Key;
        public double Value;
        public EntityPropertyListItem[] UnknownList;
    }

    public struct EntityPropertyListItem
    {
        public long UUID;
        public double Amount;
        public byte Operation;
    }
}
