namespace MineLib.Network.Modern
{
    public static class Converter
    {
        public static int[] ConvertUShort(ushort _ushort)
        {
            var intArray = new int[15];
            for (var i = 0; i < 15; i++)
            {
                if ((_ushort & (1 << i)) > 0)
                    intArray[i] = 1;
                else
                    intArray[i] = 0;
            }
            return intArray;
        }
    }
}
