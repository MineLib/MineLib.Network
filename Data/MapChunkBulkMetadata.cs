namespace MineLib.Network.Data
{
    public struct MapChunkBulkMetadata
    {
        public int ChunkX;
        public int ChunkZ;
        public ushort PrimaryBitMap;
        public ushort AddBitMap;
        public bool SkyLight;
    }
}
