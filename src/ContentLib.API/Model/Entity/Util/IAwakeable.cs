using UnityEngine;

namespace ContentLib.API.Model.Entity.Util;

/// <summary>
/// Interface Representing the general functionality of an instance that is "Awakeable", i.e. starts the game in a
/// dormant state, and then "awakes". 
/// </summary>
public interface IAwakeable
{
    /// <summary>
    /// The prefab representing the "Dormant" state of the instance.
    /// </summary>
    GameObject DormantPrefab { get; }
    
    /// <summary>
    /// The prefab representing the "Awake" state of the instance.
    /// </summary>
    GameObject? AwakePrefab { get; }
    
    /// <summary>
    /// The current "Awake" status of the instance. 
    /// </summary>
    bool IsAwake { get; set; }
       
}