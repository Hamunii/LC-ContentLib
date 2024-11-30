using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BepInEx.Configuration;

namespace ContentLib.Core.Utils;

public class ConfigManager
{
    /// <summary>
    /// Singleton pattern call via a lazy implementation of the manager.
    /// </summary>
    private static readonly Lazy<ConfigManager> instance = new Lazy<ConfigManager>(() => new ConfigManager());

    /// <summary>
    /// Gets the singleton instance of the manager.
    /// </summary>
    public static ConfigManager Instance => instance.Value;

    /// <summary>
    /// The configuration file of the API.
    /// </summary>
    private ConfigFile? _configFile;
    /// <summary>
    /// A Dictionary of Configuration Key entries, corresponding to Config Entry Containers, for the purposes of
    /// containing the various sections, keys and values of configuration file entries. 
    /// </summary>
    private readonly Dictionary<ConfigKey, ConfigEntryContainer> _configValues = new();


    /// <summary>
    /// Initializes the configuration file for the API, ensuring that all callable properties are properly initialised
    /// and ready for reference when called for.
    /// </summary>
    /// <param name="configFile">The configuration file of the API</param>
    public void InitConfig(ConfigFile configFile)
    {
        _configFile = configFile;
        foreach (ConfigKey configKey in Enum.GetValues(typeof(ConfigKey)))
        {
            var entryContainer = new ConfigEntryContainer()
            {
                Key = KeyToString(configKey), Section = KeyToSection(configKey)
            };
            entryContainer.Value = configFile.Bind(entryContainer.Section, entryContainer.Key, false).Value;
            
            _configValues.Add(configKey, entryContainer);
        }
    }


    public T? GetConfigValue<T>(ConfigKey key)
    {
        if(!_configValues.TryGetValue(key, out ConfigEntryContainer? configEntryContainer))
            throw new KeyNotFoundException($"No config entry found for key {key}");
        CLLogger.Instance.Log($"Getting config value for key {key} with value: {configEntryContainer.Value}");
        return (T)configEntryContainer.Value;
    }

    public void SetConfigValue<T>(ConfigKey key, T value)
    {
        if (!IsConfigLoaded())
            throw new NullReferenceException("Config file was not initialized!");
        
        ConfigEntryContainer? entryContainer = _configValues[key];

        if (_configFile?.TryGetEntry(entryContainer.Section, entryContainer.Key, out ConfigEntry<T> entry) != null)
        {
            entryContainer.Value = value;
            entry.Value = value;
            CLLogger.Instance.DebugLog($"Set config value for key {key} with value: {entry.Value}");
            return;
        }
        throw new KeyNotFoundException($"No config entry found for key {key}");
        
        
        
        
    }

    public void SaveConfig()
    {
        if(!IsConfigLoaded())
            throw new NullReferenceException("Config file was not initialized!");
        _configFile?.Save();
        CLLogger.Instance.Log("Config has been saved!");
    }
    
    private bool IsConfigLoaded() => _configFile != null;

    public static string KeyToString(ConfigKey configKey)
    {
        var keyString = configKey.ToString();
            
        return Regex.Replace(keyString, "(?<!^)([A-Z])", " $1");
    }

    public static string KeyToSection(ConfigKey configKey)
    {
        string key = configKey.ToString();

   
        var words = Regex.Matches(key, "[A-Z][a-z]*")
            .Select(match => match.Value)
            .ToArray();

 
        return words[^1];
    }

}

public enum ConfigKey
{
    #region Logging

    DefaultLogging, 
    PlayerEventLogging, 
    EntityEventLogging, 
    MoonEventLogging,
    ItemEventLogging, 
    CoreEventLogicLogging, 
    ModLogicEventLogging, 

    #endregion
}

public class ConfigEntryContainer()
{
    public string Section { get; set; }
    public string Key { get; set; }
    public object Value { get; set; }
}