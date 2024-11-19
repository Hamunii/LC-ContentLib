using System;
using System.Collections;
using ContentLib.API.Model.Entity.Player;
using ContentLib.API.Model.Event;
using ContentLib.Core.Utils;
using ContentLib.EnemyAPI.Events;
using ContentLib.entityAPI.Model.entity;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.ProBuilder;
using PlayerControllerB = GameNetcodeStuff.PlayerControllerB;

namespace ContentLib.EnemyAPI.Patches;

public class PlayerPatches
{
    public static void Init()
    {
        On.GameNetcodeStuff.PlayerControllerB.Start += PlayerControllerBOnStart;
        On.GameNetcodeStuff.PlayerControllerB.PlayerJumpedServerRpc += PlayerControllerBOnPlayerJumpedServerRpc;
        //On.GameNetcodeStuff.PlayerControllerB.PlayerJumpedClientRpc += PlayerControllerBOnPlayerJumpedClientRpc;
        On.GameNetcodeStuff.PlayerControllerB.PlayerJump += PlayerControllerBOnPlayerJump;
    }

    private static IEnumerator PlayerControllerBOnPlayerJump(On.GameNetcodeStuff.PlayerControllerB.orig_PlayerJump orig, PlayerControllerB self)
    {
        IEnumerator result = orig(self);
        CLLogger.Instance.Log($"Player with id {self.NetworkObjectId} has jumped!");
        GameEventManager.Instance.Trigger(new BasePlayerJumpEvent((IPlayer) EntityManager.Instance.GetEntity(self.NetworkObjectId)));
     
        return result;
    }

    private static void PlayerControllerBOnPlayerJumpedClientRpc(On.GameNetcodeStuff.PlayerControllerB.orig_PlayerJumpedClientRpc orig, PlayerControllerB self)
    {
        orig(self);
        CLLogger.Instance.Log("Player Jumped Client");
    }

    private static void PlayerControllerBOnPlayerJumpedServerRpc(On.GameNetcodeStuff.PlayerControllerB.orig_PlayerJumpedServerRpc orig, PlayerControllerB self)
    {
        var isServerExec = self.__rpc_exec_stage == NetworkBehaviour.__RpcExecStage.Server;
        orig(self);
        CLLogger.Instance.Log($"Player with id {self.NetworkObjectId} has jumped");
        if (!isServerExec)
            return;
        GameEventManager.Instance.Trigger(new BasePlayerJumpEvent((IPlayer) EntityManager.Instance.GetEntity(self.NetworkObjectId)));
    }

    private static void PlayerControllerBOnStart(On.GameNetcodeStuff.PlayerControllerB.orig_Start orig,
        PlayerControllerB self)
    {
        orig(self);
        IPlayer player = new BasePlayerInstance(self);
        EntityManager.Instance.RegisterEntity(player);
        GameEventManager.Instance.Trigger(new BasePlayerSpawnEvent(player));
    }

    private class BasePlayerInstance(PlayerControllerB playerController) : IPlayer
    {
        //TODO system worked okay, but only teleported currently selected player.And was not shown locally per client
        private ShipTeleporter Teleporter => TeleporterPatches.Instance.ShipTeleporter;
        public ulong Id => playerController.NetworkObjectId;
        public bool IsAlive => !playerController.isPlayerDead;
        public int Health => playerController.health;
        public Vector3 Position => playerController.serverPlayerPosition;
        public void Kill(Vector3 bodyVelocity,
            bool spawnBody = true,
            CauseOfDeath causeOfDeath = CauseOfDeath.Unknown,
            int deathAnimation = 0,
            Vector3 positionOffset = default) => playerController.KillPlayer(bodyVelocity,spawnBody, causeOfDeath, 
            deathAnimation, positionOffset);

        public void Revive(Vector3 revivePosition) => throw new NotImplementedException();
        public void Teleport(Vector3 pos, bool withRotation = false, float rot = 0.0f, bool allowInteractTrigger = false,
            bool enableController = true) => playerController.TeleportPlayer(pos, withRotation, 
            rot, allowInteractTrigger,enableController);

        public void TeleportToShip()
        {
            if (Teleporter == null)
            {
                CLLogger.Instance.Log("Teleporter is null, teleporting not executed.");
                return;
            }

            //TODO fix this logic? Maybe move to teleporter patches?
            if (EntityManager.Instance.EntityBeingTeleported)
            {
                //return;
            }
            
            EntityManager.Instance.EntityBeingTeleported = true;
            //TODO This works now but needs a sanity check to prevent overriding the teleport.
            StartOfRound.Instance.mapScreen.SwitchRadarTargetAndSync(SearchForPlayerInRadar(Id));
            CLLogger.Instance.Log($"Teleporting player with ID {SearchForPlayerInRadar(Id)}");
            Teleporter.StartCoroutine(DelayedTeleport(Teleporter));
        }

        public float Stamina
        {
            get => playerController.sprintMeter;
            set => playerController.sprintMeter = value;
        }

        public int HeldItemsAmount
        {
            get
            {
                var heldItems = 0;
                foreach (GrabbableObject? t in playerController.ItemSlots)
                {
                    if (t == null) continue;
                    heldItems++;
                    CLLogger.Instance.Log($"The item found was: {t.itemProperties.itemName}");
                }
                return heldItems;
            }
        }

        public bool OnTerminal => playerController.inTerminalMenu;
        public void HealOverTime(int healAmount, float healDurationInSeconds) => throw new NotImplementedException();

        public void Heal(int healAmount) => playerController.health += healAmount;
        protected virtual IEnumerator DelayedTeleport(ShipTeleporter tele)
        {
            yield return new WaitForSeconds(0.15f);
            tele.PressTeleportButtonOnLocalClient();
        }
        private int SearchForPlayerInRadar(ulong networkObjectId)
        {
            int count = StartOfRound.Instance.mapScreen.radarTargets.Count;
            CLLogger.Instance.Log($"Searching for player in radar entires: {count}");
            var thisPlayersIndex = -1;
            for (int i = 0; i < count; i++)
            {
                PlayerControllerB currentPlayer = StartOfRound.Instance.mapScreen.radarTargets[i].transform
                    .gameObject.GetComponent<PlayerControllerB>();
                if (currentPlayer.NetworkObjectId != networkObjectId)
                {
                    CLLogger.Instance.Log($"{currentPlayer.NetworkObjectId} is not the same as {networkObjectId}");
                    continue;
                }

                thisPlayersIndex = i;
                break;
            }
            return thisPlayersIndex;
        }
    }

    private class BasePlayerSpawnEvent(IPlayer player): PlayerSpawnEvent
    {
        public override IPlayer Player => player;
    }

    private class BasePlayerJumpEvent(IPlayer player) : PlayerJumpEvent
    {
        public override IPlayer Player => player;
    }
}