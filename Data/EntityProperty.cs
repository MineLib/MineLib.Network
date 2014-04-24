namespace MineLib.Network.Data
{
    public struct EntityProperty
    {
        public EntityProperty(string key, double value)
        {
            Key = key;
            Value = value;
            Modifiers = new Modifiers[0];
        }

        public string Key;
        public double Value;
        public Modifiers[] Modifiers;
    }

    public struct Modifiers
    {
        public long UUID_1;
        public long UUID_2;
        public double Amount;
        public byte Operation;
    }
}
