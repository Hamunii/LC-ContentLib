using UnityEngine;

namespace ContentLib.API.Model.Entity.Util;

/// <summary>
/// Interface representing the general functionality of an instance that is "Wakeable". I.e. is able to have a dormant
/// and active state.
/// </summary>
public interface IAwakeable
{
    /// <summary>
    /// Checks to see if the Instance is currently awake (i.e. not in its dormant state).
    /// <i>True if the instance is awake, false otherwise.</i>
    /// </summary>
    bool IsAwake { get; }
    
    /// <summary>
    /// The amount of time (in seconds) since the instance has entered an active (awake) state.
    /// </summary>
    float TimeSinceAwake { get; }
    
    /// <summary>
    /// The prefab of the instance whilst in its dormant state.
    /// </summary>
    GameObject DormantPrefab { get; }
    
    /// <summary>
    /// The prefab of the instance whilst in its active state.
    /// </summary>
    GameObject ActivePrefab { get; }
}