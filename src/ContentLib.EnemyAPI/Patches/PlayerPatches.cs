using System;
using System.Collections;
using ContentLib.API.Model.Entity.Player;
using ContentLib.API.Model.Event;
using ContentLib.Core.Utils;
using ContentLib.EnemyAPI.Events;
using Unity.Netcode;
using UnityEngine;
using PlayerControllerB = GameNetcodeStuff.PlayerControllerB;

namespace ContentLib.EnemyAPI.Patches;

public class PlayerPatches
{
    public static void Init()
    {
        On.GameNetcodeStuff.PlayerControllerB.Start += PlayerControllerBOnStart;
        On.GameNetcodeStuff.PlayerControllerB.PlayerJumpedServerRpc += PlayerControllerBOnPlayerJumpedServerRpc;
        On.GameNetcodeStuff.PlayerControllerB.PlayerJumpedClientRpc += PlayerControllerBOnPlayerJumpedClientRpc;
        On.GameNetcodeStuff.PlayerControllerB.PlayerJump += PlayerControllerBOnPlayerJump;
    }

    private static IEnumerator PlayerControllerBOnPlayerJump(On.GameNetcodeStuff.PlayerControllerB.orig_PlayerJump orig, PlayerControllerB self)
    {
        IEnumerator result = orig(self);
        CLLogger.Instance.Log("Player Jumped neither client or server");
        GameEventManager.Instance.Trigger(new BasePlayerJumpEvent(new BasePlayerInstance(self)));
     
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
        CLLogger.Instance.Log("PlayerControllerBOnPlayerJumpedServerRpc");
        if (!isServerExec)
            return;
        GameEventManager.Instance.Trigger(new BasePlayerJumpEvent(new BasePlayerInstance(self)));
    }

    private static void PlayerControllerBOnStart(On.GameNetcodeStuff.PlayerControllerB.orig_Start orig,
        PlayerControllerB self)
    {
        orig(self);
        IPlayer player = new BasePlayerInstance(self);
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

        //TODO check this system later to see why its an IEn
        public void TeleportToShip()
        {
            if (Teleporter == null)
            {
                CLLogger.Instance.Log("Teleporter is null, teleporting not executed.");
                return;
            }
            PlayerControllerB currentPlayer = StartOfRound.Instance.mapScreen.targetedPlayer;
            StartOfRound.Instance.mapScreen.targetedPlayer = playerController;
            Teleporter.PressTeleportButtonServerRpc();
            StartOfRound.Instance.mapScreen.targetedPlayer = currentPlayer;
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