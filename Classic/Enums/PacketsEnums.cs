namespace MineLib.Network.Classic.Enums
{
    public enum PacketsClient : byte
    {
        PlayerIdentification            = 0x00,
        SetBlock                        = 0x05,
        PositionAndOrientation          = 0x08,
        Message                         = 0x0D
    }

    public enum PacketsServer : byte
    {
        ServerIdentification            = 0x00,
        Ping                            = 0x01,
        LevelInitialize                 = 0x02,
        LevelDataChunk                  = 0x03,
        LevelFinalize                   = 0x04,
        SetBlock                        = 0x06,
        SpawnPlayer                     = 0x07,
        PositionAndOrientationTeleport  = 0x08,
        PositionAndOrientationUpdate    = 0x09,
        PositionUpdate                  = 0x0A,
        OrientationUpdate               = 0x0B,
        DespawnPlayer                   = 0x0C,
        Message                         = 0x0D,
        DisconnectPlayer                = 0x0E,
        UpdateUserType                  = 0x0F
    }

}

