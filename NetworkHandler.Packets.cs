using MineLib.Network.Enums;
using MineLib.Network.Packets;

namespace MineLib.Network
{
    public partial class NetworkHandler
    {
        private void RaisePacketHandled(IPacket packet, int id, ServerState state)
        {
            if (OnPacketHandled != null)
                OnPacketHandled(packet, id, state);
        }

        private void RaisePacketHandledUnUsed(IPacket packet, int id, ServerState state)
        {
            switch (state)
            {
                case ServerState.Login:

                    #region Login

                    switch ((PacketsServer) id)
                    {
                        case PacketsServer.LoginDisconnect:
                            FireLoginDisconnect(packet);
                            break;

                        case PacketsServer.EncryptionRequest:
                            EnableEncryption(packet); // Automatic enable encryption.
                            break;

                        case PacketsServer.LoginSuccess:
                            FireLoginSuccess(packet);
                            break;
                    }

                    #endregion Login

                    break;

                case ServerState.Play:

                    #region Play

                    switch ((PacketsServer) id)
                    {
                        case PacketsServer.KeepAlive:
                            FireKeepAlive(packet);
                            break;

                        case PacketsServer.JoinGame:
                            FireJoinGame(packet);
                            break;

                        case PacketsServer.ChatMessage:
                            FireChatMessage(packet);
                            break;

                        case PacketsServer.TimeUpdate:
                            FireTimeUpdate(packet);
                            break;

                        case PacketsServer.EntityEquipment:
                            FireEntityEquipment(packet);
                            break;

                        case PacketsServer.SpawnPosition:
                            FireSpawnPosition(packet);
                            break;

                        case PacketsServer.UpdateHealth:
                            FireUpdateHealth(packet);
                            break;

                        case PacketsServer.Respawn:
                            FireRespawn(packet);
                            break;

                        case PacketsServer.PlayerPositionAndLook:
                            FirePlayerPositionAndLook(packet);
                            break;

                        case PacketsServer.HeldItemChange:
                            FireHeldItemChange(packet);
                            break;

                        case PacketsServer.UseBed:
                            FireUseBed(packet);
                            break;

                        case PacketsServer.Animation:
                            FireAnimation(packet);
                            break;

                        case PacketsServer.SpawnPlayer:
                            FireSpawnPlayer(packet);
                            break;

                        case PacketsServer.CollectItem:
                            FireCollectItem(packet);
                            break;

                        case PacketsServer.SpawnObject:
                            FireSpawnObject(packet);
                            break;

                        case PacketsServer.SpawnMob:
                            FireSpawnMob(packet);
                            break;

                        case PacketsServer.SpawnPainting:
                            FireSpawnPainting(packet);
                            break;

                        case PacketsServer.SpawnExperienceOrb:
                            FireSpawnExperienceOrb(packet);
                            break;

                        case PacketsServer.EntityVelocity:
                            FireEntityVelocity(packet);
                            break;

                        case PacketsServer.DestroyEntities:
                            FireDestroyEntities(packet);
                            break;

                        case PacketsServer.Entity:
                            FireEntity(packet);
                            break;

                        case PacketsServer.EntityRelativeMove:
                            FireEntityRelativeMove(packet);
                            break;

                        case PacketsServer.EntityLook:
                            FireEntityLook(packet);
                            break;

                        case PacketsServer.EntityLookAndRelativeMove:
                            FireEntityLookAndRelativeMove(packet);
                            break;

                        case PacketsServer.EntityTeleport:
                            FireEntityTeleport(packet);
                            break;

                        case PacketsServer.EntityHeadLook:
                            FireEntityHeadLook(packet);
                            break;

                        case PacketsServer.EntityStatus:
                            FireEntityStatus(packet);
                            break;

                        case PacketsServer.AttachEntity:
                            FireAttachEntity(packet);
                            break;

                        case PacketsServer.EntityMetadata:
                            FireEntityMetadata(packet);
                            break;

                        case PacketsServer.EntityEffect:
                            FireEntityEffect(packet);
                            break;

                        case PacketsServer.RemoveEntityEffect:
                            FireRemoveEntityEffect(packet);
                            break;

                        case PacketsServer.SetExperience:
                            FireSetExperience(packet);
                            break;

                        case PacketsServer.EntityProperties:
                            FireEntityProperties(packet);
                            break;

                        case PacketsServer.ChunkData:
                            FireChunkData(packet);
                            break;

                        case PacketsServer.MultiBlockChange:
                            FireMultiBlockChange(packet);
                            break;

                        case PacketsServer.BlockChange:
                            FireBlockChange(packet);
                            break;

                        case PacketsServer.BlockAction:
                            FireBlockAction(packet);
                            break;

                        case PacketsServer.BlockBreakAnimation:
                            FireBlockBreakAnimation(packet);
                            break;

                        case PacketsServer.MapChunkBulk:
                            FireMapChunkBulk(packet);
                            break;

                        case PacketsServer.Explosion:
                            FireExplosion(packet);
                            break;

                        case PacketsServer.Effect:
                            FireEffect(packet);
                            break;

                        case PacketsServer.SoundEffect:
                            FireSoundEffect(packet);
                            break;

                        case PacketsServer.Particle:
                            FireParticle(packet);
                            break;

                        case PacketsServer.ChangeGameState:
                            FireChangeGameState(packet);
                            break;

                        case PacketsServer.SpawnGlobalEntity:
                            FireSpawnGlobalEntity(packet);
                            break;

                        case PacketsServer.OpenWindow:
                            FireOpenWindow(packet);
                            break;

                        case PacketsServer.CloseWindow:
                            FireCloseWindow(packet);
                            break;

                        case PacketsServer.SetSlot:
                            FireSetSlot(packet);
                            break;

                        case PacketsServer.WindowItems:
                            FireWindowItems(packet);
                            break;

                        case PacketsServer.WindowProperty:
                            FireWindowProperty(packet);
                            break;

                        case PacketsServer.ConfirmTransaction:
                            FireConfirmTransaction(packet);
                            break;

                        case PacketsServer.UpdateSign:
                            FireUpdateSign(packet);
                            break;

                        case PacketsServer.Maps:
                            FireMaps(packet);
                            break;

                        case PacketsServer.UpdateBlockEntity:
                            FireUpdateBlockEntity(packet);
                            break;

                        case PacketsServer.SignEditorOpen:
                            FireSignEditorOpen(packet);
                            break;

                        case PacketsServer.Statistics:
                            FireStatistics(packet);
                            break;

                        case PacketsServer.PlayerListItem:
                            FirePlayerListItem(packet);
                            break;

                        case PacketsServer.PlayerAbilities:
                            FirePlayerAbilities(packet);
                            break;

                        case PacketsServer.TabComplete:
                            FireTabComplete(packet);
                            break;

                        case PacketsServer.ScoreboardObjective:
                            FireScoreboardObjective(packet);
                            break;

                        case PacketsServer.UpdateScore:
                            FireUpdateScore(packet);
                            break;

                        case PacketsServer.DisplayScoreboard:
                            FireDisplayScoreboard(packet);
                            break;

                        case PacketsServer.Teams:
                            FireTeams(packet);
                            break;

                        case PacketsServer.PluginMessage:
                            FirePluginMessage(packet);
                            break;

                        case PacketsServer.Disconnect:
                            FireDisconnect(packet);
                            break;
                    }

                    #endregion Play

                    break;

                case ServerState.Status:

                    #region Status

                    switch ((PacketsServer) id)
                    {
                        case PacketsServer.Response:
                            FireResponse(packet);
                            break;

                        case PacketsServer.Ping:
                            FirePing(packet);
                            break;
                    }

                    #endregion Status

                    break;

                default:
                    if (OnPacketHandled != null)
                        OnPacketHandled(packet, id, state);
                    break;
            }
        }
    }
}