namespace MineLib.Network.Data.Structs
{
    public enum PlaverMovedMode
    {
        OnGround,
        Vector3,
        YawPitch,
        All
    }

    public struct PlaverMovedData
    {
        public PlaverMovedMode Mode { get; set; }
        public Vector3 Vector3 { get; set; }
        public bool OnGround { get; set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }
    }
}