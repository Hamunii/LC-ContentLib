using System;
using System.Runtime.CompilerServices.Model.Managers;
using ContentLib.API.Model;
using UnityEngine;

namespace ContentLib.Core.Loader;

public class APILoader: MonoBehaviour, IAPILoader
{
    public IEntityManager EntityManager => entityAPI.Model.entity.EntityManager.Instance;
    public IGameEventManager GameEventManager => API.Model.Event.GameEventManager.Instance;
    public IItemManager ItemManager => Item_Module.Model.ItemManager.Instance;
}