
using BepInEx;
using BepInEx.Logging;
using ContentLib.API.Model.Item;
using ContentLib.Core.Utils;
using ContentLib.Item_Module.Patches;
using ContentLib.Item_Module.Patches.Scrap;

namespace ContentLib.Item_Module;


/// <summary>
/// The Plugin instance of ContentLib.Item_Module.
/// </summary>
/// <exclude />
[BepInPlugin(LCMPluginInfo.PLUGIN_GUID, LCMPluginInfo.PLUGIN_NAME, LCMPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource s_log = null!;

    private void Awake(){
        CLLogger.Instance.Log("Loading Content-Lib Item Module...");
        InitPatches();
        CLLogger.Instance.Log("Content Lib Item Module Loaded!");
    }

    private void InitPatches()
    { 
        CLLogger.Instance.Log("Initializing Item Patches");
        ItemRoundPatches.Init();
        ItemPatches.Init();
        FlashlightPatches.Init();
        KeyPatches.Init();
        RemotePatches.Init();
        WhoopieCushionPatches.Init();
    }
}
