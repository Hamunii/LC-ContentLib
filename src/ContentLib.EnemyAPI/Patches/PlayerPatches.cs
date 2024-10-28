using System;
using ContentLib.API.Model.Entity.Player;
using ContentLib.Core.Utils;
using IL.GameNetcodeStuff;
using UnityEngine;
using PlayerControllerB = GameNetcodeStuff.PlayerControllerB;

namespace ContentLib.EnemyAPI.Patches;

public class PlayerPatches
{
    public static void Init()
    {
        On.GameNetcodeStuff.PlayerControllerB.Start += PlayerControllerBOnStart;
    }

    private static void PlayerControllerBOnStart(On.GameNetcodeStuff.PlayerControllerB.orig_Start orig,
        PlayerControllerB self)
    {
        orig(self);
        IPlayer player = new BasePlayerInstance(self);

    }

    private class BasePlayerInstance(PlayerControllerB playerController) : IPlayer
    {
        public ulong Id => playerController.NetworkObjectId;
        public bool IsAlive => !playerController.isPlayerDead;
        public int Health => playerController.health;
        public Vector3 Position => playerController.serverPlayerPosition;
        public void Kill() => throw new NotImplementedException(); //TODO needs more looking into as quite complex.

        public void Revive(Vector3 revivePosition) => throw new NotImplementedException();
        public void Teleport(Vector3 position) => throw new NotImplementedException();

        public void TeleportToShip() => throw new NotImplementedException(); //playerController.TeleportPlayer() <-- use later when params are determined. 

        public float Stamina
        {
            get => playerController.sprintMeter;
            set => throw new NotImplementedException(); //TODO needs to be a decent logic as it cant go over max etc
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
}