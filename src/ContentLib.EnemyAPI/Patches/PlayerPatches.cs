using System;
using System.Collections;
using ContentLib.API.Exceptions.Core.Manager;
using ContentLib.API.Model.Entity.Player;
using ContentLib.API.Model.Event;
using ContentLib.Core.Model.Managers;
using ContentLib.Core.Utils;
using Unity.Netcode;
using UnityEngine;
using PlayerControllerB = GameNetcodeStuff.PlayerControllerB;

namespace ContentLib.EnemyAPI.Patches;
/// <summary>
/// Patches related to general actions involving BPlayerController instances. This includes input based actions,
/// spawning, death, etc. 
/// </summary>
public class PlayerPatches
{
    public static void Init()
    {
        On.GameNetcodeStuff.PlayerControllerB.Start += PlayerControllerBOnStart;
        On.GameNetcodeStuff.PlayerControllerB.PlayJumpAudio += PlayerControllerBOnPlayJumpAudio;
     }

    private static void PlayerControllerBOnPlayJumpAudio(On.GameNetcodeStuff.PlayerControllerB.orig_PlayJumpAudio orig, PlayerControllerB self)
    {
        CLLogger.Instance.Log("TESTING JUMPY AUDIO THING YAY!");
        var player = (IPlayer) EntityManager.Instance.GetEntity(self.NetworkObjectId);
        CLLogger.Instance.DebugLog($"Player with Id '{player.Id}' jumped via Jump Audio", DebugLevel.EntityEvent);
        GameEventManager.Instance.Trigger(new BasePlayerJumpEvent(player));
        orig(self);
    }

    /// <summary>
    /// <p>Patch for registering the player instances with the Entity Manager, upon unity object start. </p>
    /// <i> Developer Note: These are, counter intuitively, registered at startup of a server, with logging in players
    /// just occupying the slot of one of these pre-registered Players.</i>
    /// </summary>
    /// <param name="orig">The original start method.</param>
    /// <param name="self">The player controller instance calling the start method.</param>
    private static void PlayerControllerBOnStart(On.GameNetcodeStuff.PlayerControllerB.orig_Start orig,
        PlayerControllerB self)
    {
        orig(self);
        IPlayer player = new BasePlayerInstance(self);
        try
        {
            EntityManager.Instance.RegisterEntity(player);
            GameEventManager.Instance.Trigger(new BasePlayerSpawnEvent(player));
        }
        catch (InvalidEntityRegistrationException exception)
        {
            CLLogger.Instance.DebugLog(exception.ToString(), DebugLevel.EntityEvent);
        }
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
            
            if (Teleporter.cooldownTime > 0.0f)
            {
                CLLogger.Instance.Log($"Cooldown Timer Value: {Teleporter.cooldownTime}");
                CLLogger.Instance.Log("Teleporter cooldown time is greater than 0.");
                return;
            }
            
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