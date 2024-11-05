
using BepInEx;
using BepInEx.Logging;
using ContentLib.Core.Utils;
using ContentLib.Item_Module.Patches;

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
    }

    private void InitPatches()
    { 
        CLLogger.Instance.Log("Initializing Item Patches");
        ItemRoundPatches.Init();
        FlashlightPatches.Init();
    }
}
