using System.Collections;
using MineLib.Network.IO;

namespace MineLib.Network.Modern.Data
{
    public struct DisplayedSkinParts
    {
        public bool CapeEnabled;
        public bool JackedEnabled;
        public bool LeftSleeveEnabled;
        public bool RightSleeveEnabled;
        public bool LeftPantsEnabled;
        public bool RightPantsEnabled;
        public bool HatEnabled;
        public bool Unused;

        public static DisplayedSkinParts FromReader(MinecraftDataReader reader)
        {
            var value = reader.ReadByte();

            return FromByte(value);
        }

        public static DisplayedSkinParts FromByte(byte value)
        {
            var bitArray = new BitArray(new byte[value]);
            var boolArray = new bool[7];
            bitArray.CopyTo(boolArray, 0);

            return new DisplayedSkinParts
            {
                CapeEnabled = boolArray[0],
                JackedEnabled = boolArray[1],
                LeftSleeveEnabled = boolArray[2],
                RightSleeveEnabled = boolArray[3],
                LeftPantsEnabled = boolArray[4],
                RightPantsEnabled = boolArray[5],
                HatEnabled = boolArray[6],
                Unused = boolArray[7]
            };
        }

        public void ToStream(MinecraftStream stream)
        {
            var value = ToByte();

            stream.WriteByte(value);
        }

        public byte ToByte()
        {
            var bitArray = new BitArray(new bool[] { CapeEnabled, JackedEnabled, LeftSleeveEnabled, RightSleeveEnabled, LeftPantsEnabled, RightPantsEnabled, HatEnabled, Unused });
            var byteArray = new byte[1];
            bitArray.CopyTo(byteArray, 0);

            return byteArray[0];
        }
    }
}
