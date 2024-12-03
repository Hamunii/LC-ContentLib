using ContentLib.API.Model.Entity;
using ContentLib.API.Model.Entity.Player;
using ContentLib.API.Model.Event;
using ContentLib.API.Model.Item;
using ContentLib.API.Model.Item.Tools.Types;
using ContentLib.Core.Model.Managers;
using ContentLib.Core.Utils;
using UnityEngine;

namespace ContentLib.Item_Module.Patches;
/// <summary>
/// Patches related to general actions involving Flashlight instances.
/// </summary>
public class FlashlightPatches
{
    public static void Init()
    {
        CLLogger.Instance.Log("FlashLight Patches");
        On.FlashlightItem.Start += FlashlightItemOnStart;
        CLLogger.Instance.Log("Flashlight Patches Complete!");
    }

  /// <summary>
    /// Patch for registering the flashlight item with the item manager.
    /// </summary>
    /// <param name="orig">The original start method being patched.</param>
    /// <param name="self">The flashlight calling the start method.</param>
    private static void FlashlightItemOnStart(On.FlashlightItem.orig_Start orig, FlashlightItem self)
    {
        orig(self);
        IGameItem gameItem = new BaseFlashlightItem(self);
        ItemManager.Instance.RegisterItem(gameItem);
    }

    private class BaseFlashlightItem(FlashlightItem flashlightItem) : IFlashlight
    {
        public ulong Id => flashlightItem.NetworkObjectId;
        public string Name => "Flashlight";

        public float Weight
        {
            get => flashlightItem.itemProperties.weight;
            set => flashlightItem.itemProperties.weight = value;
        }

        public bool IsOnShip => flashlightItem.isInShipRoom;
        public Vector3 Location => flashlightItem.transform.position;

        public void Activate()
        {
            flashlightItem.SwitchFlashlight(true);
            ItemActivationEvent activationEvent = new FlashLightActivateEvent(this);
            GameEventManager.Instance.Trigger(activationEvent);
        }
        public IGameEntity? Owner { get; set; }
    }

    private class FlashLightActivateEvent(IFlashlight? flashlight) : ItemActivationEvent
    {
        public override Vector3 Position => flashlight.Location;
        public override IGameItem? Item => flashlight;
    }
}