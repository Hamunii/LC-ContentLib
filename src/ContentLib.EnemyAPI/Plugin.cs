using BepInEx;
using BepInEx.Logging;
using ContentLib.API.Model.Event;
using ContentLib.Core.Utils;
using ContentLib.EnemyAPI.Patches;
using ContentLib.EnemyAPI.Test;

namespace ContentLib.EnemyAPI;

/// <summary>
/// The Plugin instance of ContentLib.EnemyAPI.
/// </summary>
/// <exclude />
[BepInPlugin(LCMPluginInfo.PLUGIN_GUID, LCMPluginInfo.PLUGIN_NAME, LCMPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource s_log = null!;

    private void Awake()
    {
        s_log = Logger;
        CLLogger.Instance.Log("Loading Content-Lib Entity Module!");
        InitPatches();
        CLLogger.Instance.Log("Content-Lib Entity Module Loaded!");
    }

    private void InitPatches()
    {
        CLLogger.Instance.Log("Initializing Entity Patches!");
        RoundPatches.Init();
        EnemyAIPatches.Init();
        BrackenPatches.Init();
        EyelessDogPatches.Init();
        PlayerPatches.Init();
        TeleporterPatches teleporterPatches = TeleporterPatches.Instance;
        CLLogger.Instance.Log("Enemy Patches Initialized!");
    }
}
