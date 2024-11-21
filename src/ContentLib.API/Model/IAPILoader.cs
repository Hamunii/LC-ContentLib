using System.Runtime.CompilerServices.Model.Managers;

namespace ContentLib.API.Model;
/// <summary>
/// Loader for the key API managers. Utilised within initial API loading, allowing access to Content-Lib Core Module
/// functionality. 
/// </summary>
public interface IAPILoader
{
    /// <summary>
    /// The Entity Manager, responsible for in-game entity management.
    /// </summary>
    IEntityManager EntityManager { get; }
    
    /// <summary>
    /// The Game Event Manager, responsible for the triggering of in-game events and registration of listeners for
    /// said events. 
    /// </summary>
    IGameEventManager GameEventManager { get; }
    
    /// <summary>
    /// The Item Manager, responsible for in-game item management. 
    /// </summary>
    IItemManager ItemManager { get; }
}