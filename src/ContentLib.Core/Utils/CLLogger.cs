using System;
using System.Collections.Generic;

using BepInEx.Logging;

namespace ContentLib.Core.Utils
{
    /// <summary>
    /// Singleton logger utility that allows enabling/disabling of certain log types at runtime. By Default each log
    /// type will be disabled.
    /// </summary>
    public class CLLogger
    {
        private static readonly Lazy<CLLogger> instance = new (() => new CLLogger());

  
        public static CLLogger Instance => instance.Value;

        /// <summary>
        /// Private constructor that creates loggers for each of the different logging options within the API config.
        /// </summary>
        private CLLogger()
        {
            foreach (DebugLevel debugLevel in Enum.GetValues(typeof(DebugLevel)))
            {
                _logSettings[debugLevel] = DebugLevelToConfigKey(debugLevel);
                _logSources[debugLevel] = Logger.CreateLogSource("Content-Lib: "+ Enum.GetName(typeof(DebugLevel), debugLevel));
            }
        }
        /// <summary>
        /// Dictionary that directly relates Debug Level to Config Key.
        /// </summary>
        private readonly Dictionary<DebugLevel, ConfigKey> _logSettings = new();
        
        /// <summary>
        /// Dictionary that maps the log sources to their respective Debug Level. 
        /// </summary>
        private readonly Dictionary<DebugLevel, ManualLogSource> _logSources = new();

        /// <summary>
        /// Logs a message that is related to a mod that 
        /// </summary>
        /// <param name="message"></param>
        public void Log(string message) => Plugin.s_log.LogInfo(message);

        /// <summary>
        /// Logs a debug message, if the log type is enabled.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="debugLevel">The type of log.</param>
        public void DebugLog(string message, DebugLevel debugLevel = DebugLevel.Default)
        {
            if (_logSettings.TryGetValue(debugLevel, out ConfigKey configKey) 
                && 
                ConfigManager.Instance.GetConfigValue<bool>(configKey))
            {
                
                _logSources[debugLevel].LogDebug($" {message}");
            }
        }

        /// <summary>
        /// Returns the config key (if it exists) that corresponds to the given Debug Level. 
        /// </summary>
        /// <param name="debugLevel"></param>
        /// <returns></returns>
        private ConfigKey DebugLevelToConfigKey(DebugLevel debugLevel)
        {
            var debugLevelString = Enum.GetName(typeof(DebugLevel), debugLevel);
            Enum.TryParse(debugLevelString + "Logging", out ConfigKey configKey);
            return configKey;
        }
    }
    

    /// <summary>
    /// Enum representing different types of logs.
    /// </summary>
    public enum DebugLevel
    {
        /// <summary>
        /// Default Logger, for any non-specific debug logs.
        /// </summary>
        Default, 
        /// <summary>
        /// Logs to do with Player Events.
        /// </summary>
        PlayerEvent, 
        /// <summary>
        /// Logs to do with Events involving in-game entities. 
        /// </summary>
        EntityEvent, 
        /// <summary>
        /// Logs to do with Events involving Moons (such as loading of moons, spawning of creatures on moons etc.
        /// </summary>
        MoonEvent, 
        /// <summary>
        /// Logs to do with Events involving in-game items, such as picking items up, using an item's actions, etc.
        /// </summary>
        ItemEvent, 
        /// <summary>
        /// Logs to do with Content-Lib Core Module logic. 
        /// </summary>
        CoreEvent, 
        /// <summary>
        /// Logs to do with Events involving the logic of a Mod that utilises the Content-Lib API. 
        /// </summary>
        ModLogicEvent, 
    }

    
}
