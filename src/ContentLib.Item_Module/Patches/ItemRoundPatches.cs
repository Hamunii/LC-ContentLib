using ContentLib.Item_Module.Model;
using UnityEngine;

namespace ContentLib.Item_Module.Patches;
/// <summary>
/// Patches related to round-management actions involving IGameItems, such as unregistration at round-end.
/// </summary>
public class ItemRoundPatches
{
    public static void Init()
    {
        On.StartOfRound.ShipLeave += StartOfRoundOnShipLeave;
    }

    /// <summary>
    /// Patch to unregister all currently registered items (that are not persistent) at teh end of a round.
    /// </summary>
    /// <param name="orig">Original StartOfRound method for when the ship is leaving.</param>
    /// <param name="self">The StartOfRound Instance</param>
    private static void StartOfRoundOnShipLeave(On.StartOfRound.orig_ShipLeave orig, StartOfRound self)
    {
        ItemManager.Instance.UnRegisterNonPersistingItems();
    }
}