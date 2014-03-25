namespace MineLib.Network.Data
{
    public struct MapChunkBulkMetadata
    {
        public int ChunkX;
        public int ChunkZ;
        public short PrimaryBitMap;
        public short AddBitMap;
        public bool SkyLight;
    }
}
