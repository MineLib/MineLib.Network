using MineLib.Network.Events;

namespace MineLib.Network
{
    public partial class NetworkHandler
    {
        public event PacketsHandler OnPacketHandled;

        public event PacketHandler FireLoginSuccess;
        public event PacketHandler FireLoginDisconnect;

        public event PacketHandler FireResponse;
        public event PacketHandler FirePing;


        public event PacketHandler FireKeepAlive;
        public event PacketHandler FireJoinGame;
        public event PacketHandler FireChatMessage;
        public event PacketHandler FireTimeUpdate;
        public event PacketHandler FireEntityEquipment;
        public event PacketHandler FireSpawnPosition;
        public event PacketHandler FireUpdateHealth;
        public event PacketHandler FireRespawn;
        public event PacketHandler FirePlayerPositionAndLook;
        public event PacketHandler FireHeldItemChange;
        public event PacketHandler FireUseBed;
        public event PacketHandler FireAnimation;
        public event PacketHandler FireSpawnPlayer;
        public event PacketHandler FireCollectItem;
        public event PacketHandler FireSpawnObject;
        public event PacketHandler FireSpawnMob;
        public event PacketHandler FireSpawnPainting;
        public event PacketHandler FireSpawnExperienceOrb;
        public event PacketHandler FireEntityVelocity;
        public event PacketHandler FireDestroyEntities;
        public event PacketHandler FireEntity;
        public event PacketHandler FireEntityRelativeMove;
        public event PacketHandler FireEntityLook;
        public event PacketHandler FireEntityLookAndRelativeMove;
        public event PacketHandler FireEntityTeleport;
        public event PacketHandler FireEntityHeadLook;
        public event PacketHandler FireEntityStatus;
        public event PacketHandler FireAttachEntity;
        public event PacketHandler FireEntityMetadata;
        public event PacketHandler FireEntityEffect;
        public event PacketHandler FireRemoveEntityEffect;
        public event PacketHandler FireSetExperience;
        public event PacketHandler FireEntityProperties;
        public event PacketHandler FireChunkData;
        public event PacketHandler FireMultiBlockChange;
        public event PacketHandler FireBlockChange;
        public event PacketHandler FireBlockAction;
        public event PacketHandler FireBlockBreakAnimation;
        public event PacketHandler FireMapChunkBulk;
        public event PacketHandler FireExplosion;
        public event PacketHandler FireEffect;
        public event PacketHandler FireSoundEffect;
        public event PacketHandler FireParticle;
        public event PacketHandler FireChangeGameState;
        public event PacketHandler FireSpawnGlobalEntity;
        public event PacketHandler FireOpenWindow;
        public event PacketHandler FireCloseWindow;
        public event PacketHandler FireSetSlot;
        public event PacketHandler FireWindowItems;
        public event PacketHandler FireWindowProperty;
        public event PacketHandler FireConfirmTransaction;
        public event PacketHandler FireUpdateSign;
        public event PacketHandler FireMaps;
        public event PacketHandler FireUpdateBlockEntity;
        public event PacketHandler FireSignEditorOpen;
        public event PacketHandler FireStatistics;
        public event PacketHandler FirePlayerListItem;
        public event PacketHandler FirePlayerAbilities;
        public event PacketHandler FireTabComplete;
        public event PacketHandler FireScoreboardObjective;
        public event PacketHandler FireUpdateScore;
        public event PacketHandler FireDisplayScoreboard;
        public event PacketHandler FireTeams;
        public event PacketHandler FirePluginMessage;
        public event PacketHandler FireDisconnect;
    }
}