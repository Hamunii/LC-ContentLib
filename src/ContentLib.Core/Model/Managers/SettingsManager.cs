using System;
using System.Collections.Generic;
using BepInEx.Configuration;

namespace ContentLib.Core.Model.Managers;

public class SettingsManager
{
    /// <summary>
    /// Singleton pattern call via a lazy implementation of the manager.
    /// </summary>
    private static readonly Lazy<SettingsManager> instance = new Lazy<SettingsManager>(() => new SettingsManager());

    /// <summary>
    /// Gets the singleton instance of the manager.
    /// </summary>
    public static SettingsManager Instance => instance.Value;
   
    public void MoveToEventsSettingPage()
    {
        
    }
    public void MoveDependenciesToSettingPage()
    {
        
    }

}



