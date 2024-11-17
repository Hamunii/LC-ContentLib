using ContentLib.API.Model.Entity.Util;

namespace ContentLib.API.Model.Entity.Player;

/// <summary>
/// Interface that represents the general functionality of a Player within the game space.
/// </summary>
public interface IPlayer :IGameEntity, IKillable,IRevivable, IHealable, ITeleportable
{
    /// <summary>
    /// The stamina of the Player, when at 0, the Player is unable to run. 
    /// </summary>
    float Stamina { get; set; }
    
    /// <summary>
    /// The amount of items the Player is currently holding within their inventory. 
    /// </summary>
    int HeldItemsAmount { get; }
    
    /// <summary>
    /// Check that defines if a player is currently on the Terminal within the ship. 
    /// </summary>
    bool OnTerminal { get; }
    
}