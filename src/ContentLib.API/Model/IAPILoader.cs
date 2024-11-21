using System.Runtime.CompilerServices.Model.Managers;

namespace ContentLib.API.Model;

public interface IAPILoader
{
    IEntityManager EntityManager { get; }
    IGameEventManager GameEventManager { get; }
    IItemManager ItemManager { get; }
}