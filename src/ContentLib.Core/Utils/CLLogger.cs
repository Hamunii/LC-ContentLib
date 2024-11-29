using System;
using System.Collections.Generic;
using System.Linq;

namespace ContentLib.Core.Utils
{
    /// <summary>
    /// Singleton logger utility that allows enabling/disabling of certain log types at runtime. By Default each log
    /// type will be disabled.
    /// </summary>
    public class CLLogger
    {
        private static readonly Lazy<CLLogger> instance = new Lazy<CLLogger>(() => new CLLogger());

        public static CLLogger Instance => instance.Value;

        private CLLogger()
        {
            foreach (DebugLevel debugLevel in Enum.GetValues(typeof(DebugLevel)))
            {
                _logSettings[debugLevel] = DebugLevelToConfigKey(debugLevel);
            }
        }
        
        private readonly Dictionary<DebugLevel, ConfigKey> _logSettings = new();

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
                Plugin.s_log.LogMessage($"[{LCMPluginInfo.PLUGIN_NAME}-{debugLevel}] {message}");
            }
        }

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
        Default, PlayerEvent, EntityEvent, MoonEvent, ItemEvent, CoreEvent, ModLogicEvent, 
    }

    
}
