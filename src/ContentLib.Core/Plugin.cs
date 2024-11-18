using BepInEx;
using BepInEx.Logging;
using ContentLib.Core.Utils;
using ContentLib.Core.Utils.Config;

namespace ContentLib.Core;

/// <summary>
/// The Plugin instance of ContentLib.Core.
/// </summary>
/// <exclude />
[BepInPlugin(LCMPluginInfo.PLUGIN_GUID, LCMPluginInfo.PLUGIN_NAME, LCMPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource s_log = null!;

    private void Awake()
    {
        s_log = Logger;
        CLLogger.Instance.Log($"{LCMPluginInfo.PLUGIN_NAME} is loaded!");
        ConfigHandler.Instance.InitConfig(Config);
        CLLogger.Instance.DebugLog("This is a test you could put into an enemy event", DebugLevel.EnemyEvent);
    }
}
