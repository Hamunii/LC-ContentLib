using UnityEngine;

namespace ContentLib.API.Model.Item;
/// <summary>
/// Interface representing the general functionality of an in-game Item.
/// </summary>
public interface IGameItem
{
    /// <summary>
    /// The id of the item.
    /// </summary>
    ulong Id { get; }
    
    /// <summary>
    /// The name of the item.
    /// </summary>
    string Name { get;}
    
    /// <summary>
    /// The weight of the item, in lbs.
    /// </summary>
    float Weight { get; set; }
    
    /// <summary>
    /// Bool representing if the item is currently on the ship. 
    /// </summary>
    bool IsOnShip { get; }
    
    /// <summary>
    /// The location of the item within the game-world.
    /// </summary>
    Vector3 Location { get; }
}