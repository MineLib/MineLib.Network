namespace MineLib.Network.Data.Structs
{
    public enum PlayerSetRemoveBlockEnum
    {
        Remove,
        Place,
        Dig
    }

    public struct PlayerSetRemoveBlockData
    {
        public PlayerSetRemoveBlockEnum Mode { get; set; }
        public Position Location { get; set; }
        public int BlockID { get; set; }
        public byte Status { get; set; }
        public byte Face { get; set; }
        public byte Direction { get; set; }
        public ItemStack Slot { get; set; }
        public Position Crosshair { get; set; }
    }
}