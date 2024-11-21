using System;
using System.Collections.Generic;

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
            foreach (DebugLevel logType in Enum.GetValues(typeof(DebugLevel)))
            {
                _logSettings[logType] = false;
            }
        }

        private readonly Dictionary<DebugLevel, bool> _logSettings = new();

        /// <summary>
        /// Enables logging for the specified log type.
        /// </summary>
        /// <param name="debugLevel">The log type to enable.</param>
        public void EnableLogType(DebugLevel debugLevel)
        {
            _logSettings[debugLevel] = true;
        }

        /// <summary>
        /// Disables logging for the specified log type.
        /// </summary>
        /// <param name="debugLevel">The log type to disable.</param>
        public void DisableLogType(DebugLevel debugLevel)
        {
            _logSettings[debugLevel] = false;
        }

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
            if (_logSettings.TryGetValue(debugLevel, out bool isEnabled) && isEnabled)
            {
                Plugin.s_log.LogMessage($"[{LCMPluginInfo.PLUGIN_NAME}-{debugLevel}] {message}");
            }
        }
    }

    /// <summary>
    /// Enum representing different types of logs.
    /// </summary>
    public enum DebugLevel
    {
        Default, PlayerEvent, EnemyEvent, MoonEvent, ItemEvent, ModLogicEvent, 
    }
}
