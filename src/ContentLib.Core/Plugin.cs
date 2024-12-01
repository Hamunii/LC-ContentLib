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
  
    /// <summary>
    /// Initialises the primary logger for API-related logging.
    /// </summary>
    private void InitLogger()
    {
        s_log = Logger;
        CLLogger.Instance.Log($"{LCMPluginInfo.PLUGIN_NAME} Loading!");
    }
    
    /// <summary>
    /// <p>Initializes the API loader instance for the client. </p>
    /// <i>Developer Note: The loader, hooked onto a Persistent Game Object, is searched for by mods requiring it. This
    /// allows mods to be created with just the API as a dependency, whilst still being able to find the Core-Module's
    /// loader instance.</i>  
    /// </summary>
    private void InitAPI()
    {
        CLLogger.Instance.Log("Initialising API Loader!");
        var apiLoaderObject = new GameObject("APILoader");
        apiLoaderObject.AddComponent<APILoader>();
        DontDestroyOnLoad(apiLoaderObject);
        CLLogger.Instance.Log("API Loader Initialized!");
        
    }
    
    /// <summary>
    /// Initialises both the Manager responsible for Content-Lib Configuration File Settings and the in-game terminal
    /// for changing said Settings.
    /// </summary>
    private void InitConfig()
    {
        CLLogger.Instance.Log("Initialising Config Settings!");
        ConfigManager.Instance.InitConfig(Config);
        InteractiveTerminalManager.RegisterApplication<SettingsTerminal>("Settings", false);
        CLLogger.Instance.Log("Config Settings Initialized!");
    }

    /// <summary>
    /// Initialises the patches required for Core-Module logic setup.
    /// </summary>
    private void InitPatches()
    {
        CLLogger.Instance.Log("Initialising Patches!");
        StartOfRoundPatches.Init();
        CLLogger.Instance.Log("Patches Initialized!");
    }

  
}
