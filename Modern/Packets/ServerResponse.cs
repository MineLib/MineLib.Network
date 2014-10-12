using MineLib.Network.Modern.Packets.Server;
using MineLib.Network.Modern.Packets.Server.Login;
using MineLib.Network.Modern.Packets.Server.Status;

namespace MineLib.Network.Modern.Packets
{
    public struct ServerResponse
    {
        public delegate IPacket CreatePacketInstance();

        #region Login Response
        public static readonly CreatePacketInstance[] Login =
        {
            () => new LoginDisconnectPacket(),              // 0x00
            () => new EncryptionRequestPacket(),            // 0x01
            () => new LoginSuccessPacket(),                 // 0x02
            () => new Server.Login.SetCompressionPacket()   // 0x03
        };
        #endregion

        #region Status Response
        public static readonly CreatePacketInstance[] Status =
        {
            () => new ResponsePacket(),                     // 0x00
            () => new PingPacket()                          // 0x01
        };
        #endregion

        #region Play Response
        public static readonly CreatePacketInstance[] Play =
        {
            () => new KeepAlivePacket(),                    // 0x00
            () => new JoinGamePacket(),                     // 0x01
            () => new ChatMessagePacket(),                  // 0x02
            () => new TimeUpdatePacket(),                   // 0x03
            () => new EntityEquipmentPacket(),              // 0x04
            () => new SpawnPositionPacket(),                // 0x05
            () => new UpdateHealthPacket(),                 // 0x06
            () => new RespawnPacket(),                      // 0x07
            () => new PlayerPositionAndLookPacket(),        // 0x08
            () => new HeldItemChangePacket(),               // 0x09
            () => new UseBedPacket(),                       // 0x0A
            () => new AnimationPacket(),                    // 0x0B
            () => new SpawnPlayerPacket(),                  // 0x0C
            () => new CollectItemPacket(),                  // 0x0D
            () => new SpawnObjectPacket(),                  // 0x0E
            () => new SpawnMobPacket(),                     // 0x0F
            () => new SpawnPaintingPacket(),                // 0x10
            () => new SpawnExperienceOrbPacket(),           // 0x11
            () => new EntityVelocityPacket(),               // 0x12
            () => new DestroyEntitiesPacket(),              // 0x13
            () => new EntityPacket(),                       // 0x14
            () => new EntityRelativeMovePacket(),           // 0x15
            () => new EntityLookPacket(),                   // 0x16
            () => new EntityLookAndRelativeMovePacket(),    // 0x17
            () => new EntityTeleportPacket(),               // 0x18
            () => new EntityHeadLookPacket(),               // 0x19
            () => new EntityStatusPacket(),                 // 0x1A
            () => new AttachEntityPacket(),                 // 0x1B
            () => new EntityMetadataPacket(),               // 0x1C
            () => new EntityEffectPacket(),                 // 0x1D
            () => new RemoveEntityEffectPacket(),           // 0x1E
            () => new SetExperiencePacket(),                // 0x1F
            () => new EntityPropertiesPacket(),             // 0x20
            () => new ChunkDataPacket(),                    // 0x21
            () => new MultiBlockChangePacket(),             // 0x22
            () => new BlockChangePacket(),                  // 0x23
            () => new BlockActionPacket(),                  // 0x24
            () => new BlockBreakAnimationPacket(),          // 0x25
            () => new MapChunkBulkPacket(),                 // 0x26
            () => new ExplosionPacket(),                    // 0x27
            () => new EffectPacket(),                       // 0x28
            () => new SoundEffectPacket(),                  // 0x29
            () => new ParticlePacket(),                     // 0x2A
            () => new ChangeGameStatePacket(),              // 0x2B
            () => new SpawnGlobalEntityPacket(),            // 0x2C
            () => new OpenWindowPacket(),                   // 0x2D
            () => new CloseWindowPacket(),                  // 0x2E
            () => new SetSlotPacket(),                      // 0x2F
            () => new WindowItemsPacket(),                  // 0x30
            () => new WindowPropertyPacket(),               // 0x31
            () => new ConfirmTransactionPacket(),           // 0x32
            () => new UpdateSignPacket(),                   // 0x33
            () => new MapsPacket(),                         // 0x34 
            () => new UpdateBlockEntityPacket(),            // 0x35
            () => new SignEditorOpenPacket(),               // 0x36
            () => new StatisticsPacket(),                   // 0x37
            () => new PlayerListItemPacket(),               // 0x38
            () => new PlayerAbilitiesPacket(),              // 0x39
            () => new TabCompletePacket(),                  // 0x3A
            () => new ScoreboardObjectivePacket(),          // 0x3B
            () => new UpdateScorePacket(),                  // 0x3C
            () => new DisplayScoreboardPacket(),            // 0x3D
            () => new TeamsPacket(),                        // 0x3E
            () => new PluginMessagePacket(),                // 0x3F
            () => new DisconnectPacket(),                   // 0x40
            () => new ServerDifficultyPacket(),             // 0x41
            () => new CombatEventPacket(),                  // 0x42
            () => new CameraPacket(),                       // 0x43
            () => new WorldBorderPacket(),                  // 0x44
            () => new TitlePacket(),                        // 0x45
            () => new Server.SetCompressionPacket(),        // 0x46
            () => new PlayerListHeaderFooterPacket(),       // 0x47
            () => new ResourcePackSendPacket(),             // 0x48
            () => new UpdateEntityNBTPacket(),              // 0x49
            null, // 0x4A
            null, // 0x4B
            null, // 0x4C
            null, // 0x4D
            null, // 0x4E
            null, // 0x4F
            null, // 0x50
            null, // 0x51
            null, // 0x52
            null, // 0x53
            null, // 0x54
            null, // 0x55
            null, // 0x56
            null, // 0x57
            null, // 0x58
            null, // 0x59
            null, // 0x5A
            null, // 0x5B
            null, // 0x5C
            null, // 0x5D
            null, // 0x5E
            null, // 0x5F
            null, // 0x60
            null, // 0x61
            null, // 0x62
            null, // 0x63
            null, // 0x64
            null, // 0x65
            null, // 0x66
            null, // 0x67
            null, // 0x68
            null, // 0x69
            null, // 0x6A
            null, // 0x6B
            null, // 0x6C
            null, // 0x6D
            null, // 0x6E
            null, // 0x6F
            null, // 0x70
            null, // 0x71
            null, // 0x72
            null, // 0x73
            null, // 0x74
            null, // 0x75
            null, // 0x76
            null, // 0x77
            null, // 0x78
            null, // 0x79
            null, // 0x7A
            null, // 0x7B
            null, // 0x7C
            null, // 0x7D
            null, // 0x7E
            null, // 0x7F
            null, // 0x80
            null, // 0x81
            null, // 0x82
            null, // 0x83
            null, // 0x84
            null, // 0x85
            null, // 0x86
            null, // 0x87
            null, // 0x88
            null, // 0x89
            null, // 0x8A
            null, // 0x8B
            null, // 0x8C
            null, // 0x8D
            null, // 0x8E
            null, // 0x8F
            null, // 0x90
            null, // 0x91
            null, // 0x92
            null, // 0x93
            null, // 0x94
            null, // 0x95
            null, // 0x96
            null, // 0x97
            null, // 0x98
            null, // 0x99
            null, // 0x9A
            null, // 0x9B
            null, // 0x9C
            null, // 0x9D
            null, // 0x9E
            null, // 0x9F
            null, // 0xA0
            null, // 0xA1
            null, // 0xA2
            null, // 0xA3
            null, // 0xA4
            null, // 0xA5
            null, // 0xA6
            null, // 0xA7
            null, // 0xA8
            null, // 0xA9
            null, // 0xAA
            null, // 0xAB
            null, // 0xAC
            null, // 0xAD
            null, // 0xAE
            null, // 0xAF
            null, // 0xB0
            null, // 0xB1
            null, // 0xB2
            null, // 0xB3
            null, // 0xB4
            null, // 0xB5
            null, // 0xB6
            null, // 0xB7
            null, // 0xB8
            null, // 0xB9
            null, // 0xBA
            null, // 0xBB
            null, // 0xBC
            null, // 0xBD
            null, // 0xBE
            null, // 0xBF
            null, // 0xC0
            null, // 0xC1
            null, // 0xC2
            null, // 0xC3
            null, // 0xC4
            null, // 0xC5
            null, // 0xC6
            null, // 0xC7
            null, // 0xC8
            null, // 0xC9
            null, // 0xCA
            null, // 0xCB
            null, // 0xCC
            null, // 0xCD
            null, // 0xCE
            null, // 0xCF
            null, // 0xD0
            null, // 0xD1
            null, // 0xD2
            null, // 0xD3
            null, // 0xD4
            null, // 0xD5
            null, // 0xD6
            null, // 0xD7
            null, // 0xD8
            null, // 0xD9
            null, // 0xDA
            null, // 0xDB
            null, // 0xDC
            null, // 0xDD
            null, // 0xDE
            null, // 0xDF
            null, // 0xE0
            null, // 0xE1
            null, // 0xE2
            null, // 0xE3
            null, // 0xE4
            null, // 0xE5
            null, // 0xE6
            null, // 0xE7
            null, // 0xE8
            null, // 0xE9
            null, // 0xEA
            null, // 0xEB
            null, // 0xEC
            null, // 0xED
            null, // 0xEE
            null, // 0xEF
            null, // 0xF0
            null, // 0xF1
            null, // 0xF2
            null, // 0xF3
            null, // 0xF4
            null, // 0xF5
            null, // 0xF6
            null, // 0xF7
            null, // 0xF8
            null, // 0xF9
            null, // 0xFA
            null, // 0xFB
            null, // 0xFC
            null, // 0xFD
            null, // 0xFE
            null  // 0xFF
        };
        #endregion
    }
}
