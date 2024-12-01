using BepInEx;
using BepInEx.Logging;
using ContentLib.Core.Loader;
using ContentLib.Core.Model.Terminal;
using ContentLib.Core.Patches;
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
      InitLogger();
      InitConfig();
      InitAPI();
      InitPatches();
    }
  
    private void InitLogger()
    {
        s_log = Logger;
        CLLogger.Instance.Log($"{LCMPluginInfo.PLUGIN_NAME} Loading!");
    }
    
    private void InitAPI()
    {
        CLLogger.Instance.Log("Initialising API Loader!");
        var apiLoaderObject = new GameObject("APILoader");
        apiLoaderObject.AddComponent<APILoader>();
        DontDestroyOnLoad(apiLoaderObject);
        CLLogger.Instance.Log("API Loader Initialized!");
        
    }
    private void InitConfig()
    {
        CLLogger.Instance.Log("Initialising Config Settings!");
        ConfigManager.Instance.InitConfig(Config);
        InteractiveTerminalManager.RegisterApplication<SettingsTerminal>("Settings", false);
        CLLogger.Instance.Log("Config Settings Initialized!");
    }

    private void InitPatches()
    {
        CLLogger.Instance.Log("Initialising Patches!");
        StartOfRoundPatches.Init();
        CLLogger.Instance.Log("Patches Initialized!");
    }

  
}
