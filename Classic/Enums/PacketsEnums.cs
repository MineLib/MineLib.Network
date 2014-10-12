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
        UpdateUserType                  = 0x0F,
        ExtInfo                         = 0x10,
        ExtEntry                        = 0x11,
        SetClickDistance                = 0x12,
        CustomBlockSupportLevel         = 0x13,
        HoldThis                        = 0x14,
        SetTextHotKey                   = 0x15,
        ExtAddPlayerName                = 0x16,
        ExtRemovePlayerName             = 0x18,
        EnvSetColor                     = 0x19,
        MakeSelection                   = 0x1A,
        RemoveSelection                 = 0x1B,
        SetBlockPermission              = 0x1C,
        ChangeModel                     = 0x1D,
        EnvSetMapAppearance             = 0x1E,
        EnvSetWeatherType               = 0x1F,
        HackControl                     = 0x20,
        ExtAddEntity2                   = 0x21,
    }

}

