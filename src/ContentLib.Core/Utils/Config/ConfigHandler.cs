using System;
using BepInEx.Configuration;

namespace ContentLib.Core.Utils.Config;

public class ConfigHandler
{
    private static readonly Lazy<ConfigHandler> instance = new Lazy<ConfigHandler>(() => new ConfigHandler());

    public static ConfigHandler Instance => instance.Value;

    public void InitConfig(ConfigFile config)
    {
        foreach (DebugLevel debugLevel in Enum.GetValues(typeof(DebugLevel)))
        {
            var isDebugActive = config.Bind("Debug", debugLevel.ToString(),false,$"If debug messages for " +
                                                             $"{Enum.GetName(typeof(DebugLevel), debugLevel)} are shown" +
                                                             $"in the BepInEx console.");
            if(isDebugActive.Value) 
                CLLogger.Instance.EnableLogType(debugLevel);
        }
        
    }
}