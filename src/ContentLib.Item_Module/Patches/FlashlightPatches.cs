using ContentLib.API.Model.Event;
using ContentLib.API.Model.Item;
using ContentLib.API.Model.Item.Tools.Types;
using ContentLib.Core.Utils;
using ContentLib.Item_Module.Model;
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
        On.FlashlightItem.SwitchFlashlight += FlashlightItemOnSwitchFlashlight;
        CLLogger.Instance.Log("Flashlight Patches Complete!");
    }

    /// <summary>
    /// Patch for triggering activation events for the Flashlight. 
    /// </summary>
    /// <param name="orig">The original flashlight activation method.</param>
    /// <param name="self">The flashlight that called the method.</param>
    /// <param name="on">Bool representing if the flashlight was switched off or on.</param>
    private static void FlashlightItemOnSwitchFlashlight(On.FlashlightItem.orig_SwitchFlashlight orig, FlashlightItem self, bool on)
    {
        orig(self, on);
        if (on)
        {
            ItemActivationEvent activationEvent =
                new FlashLightActivateEvent((IFlashlight)ItemManager.Instance.GetItem(self.NetworkObjectId));
            GameEventManager.Instance.Trigger(activationEvent);
        }
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
    }

    private class FlashLightActivateEvent(IFlashlight flashlight) : ItemActivationEvent
    {
        public override Vector3 Position => flashlight.Location;
        public override IGameItem Item => flashlight;
    }
}