using ContentLib.API.Model.Entity;
using ContentLib.API.Model.Item;
using UnityEngine;

namespace ContentLib.API.Model.Event;
/// <summary>
/// Interface representing the general functionality of a game event that involves an IGameItem instance.
/// </summary>
public interface IItemEvent : IGameEvent
{
    /// <summary>
    /// The current position of the event.
    /// </summary>
    Vector3 Position { get; }
    /// <summary>
    /// The item involved in the event.
    /// </summary>
    IGameItem Item { get; }
}
/// <summary>
/// An event that represents the activation of an item. 
/// </summary>
public abstract class ItemActivationEvent : IItemEvent
{
  
    /// <inheritdoc/>
    public abstract Vector3 Position { get; }
    
    /// <inheritdoc/>
    public abstract IGameItem Item { get; }
    
    /// <inheritdoc />
    public bool IsCancelled { get; set; }
}

/// <summary>
/// An event that represents a game entity picking up of an item within the game world. 
/// </summary>
public abstract class ItemPickUpEvent : IItemEvent
{
    /// <inheritdoc />
    public abstract Vector3 Position { get; }
    
    /// <inheritdoc />
    public abstract IGameItem Item { get; }
    
    /// <inheritdoc />
    public bool IsCancelled { get; set; }
    
    /// <summary>
    /// The Game Entity that picked up the item.
    /// </summary>
    public abstract IGameEntity GrabbingEntity { get; } 
    
}

/// <summary>
/// An event that represents a game entity dropping an item within the game world. 
/// </summary>
public abstract class ItemDroppedEvent : IItemEvent
{
    /// <inheritdoc />
    public abstract Vector3 Position { get; }
    
    /// <inheritdoc />
    public abstract IGameItem Item { get; }
    
    /// <inheritdoc />
    public bool IsCancelled { get; set; }
    
    /// <summary>
    /// The Game Entity that dropped the item.
    /// </summary>
    public abstract IGameEntity DroppingEntity { get; } 
    
}
