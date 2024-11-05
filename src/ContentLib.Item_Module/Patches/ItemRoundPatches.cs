using ContentLib.Item_Module.Model;
using UnityEngine;

namespace ContentLib.Item_Module.Patches;
/// <summary>
/// Patches related to general actions involving IGameItems, such as deregistration at round-end.
/// </summary>
public class ItemRoundPatches
{
    public static void Init()
    {
        Debug.Log("Patching Round Methods");
        On.StartOfRound.ShipLeave += StartOfRoundOnShipLeave;
    }

    private static void StartOfRoundOnShipLeave(On.StartOfRound.orig_ShipLeave orig, StartOfRound self)
    {
        ItemManager.Instance.UnRegisterNonPersistingItems();
    }
}