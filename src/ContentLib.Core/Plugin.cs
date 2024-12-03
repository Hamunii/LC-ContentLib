using BepInEx;
using BepInEx.Logging;
using ContentLib.Core.Loader;
using ContentLib.Core.Model.Terminal;
using ContentLib.Core.Utils;
using InteractiveTerminalAPI.UI;
using UnityEngine;

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
        ConfigManager.Instance.InitConfig(Config);
        var apiLoaderObject = new GameObject("APILoader");
        apiLoaderObject.AddComponent<APILoader>();
        DontDestroyOnLoad(apiLoaderObject);
        InteractiveTerminalManager.RegisterApplication<SettingsTerminal>("Settings", false);
    }
}
