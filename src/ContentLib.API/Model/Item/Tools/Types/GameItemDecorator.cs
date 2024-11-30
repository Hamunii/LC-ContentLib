using ContentLib.API.Model.Entity;
using UnityEngine;

namespace ContentLib.API.Model.Item.Tools.Types;

public abstract class GameItemDecorator(IGameItem gameItem) : IGameItem
{
    private IGameItem _gameItem = gameItem;
    public virtual ulong Id  => _gameItem.Id;
    public virtual string Name => _gameItem.Name;
    public virtual float Weight { 
        get => _gameItem.Weight;
        set => _gameItem.Weight = value;
    }
    public virtual bool IsOnShip => _gameItem.IsOnShip;
    public virtual Vector3 Location => _gameItem.Location;
    public virtual IGameEntity? Owner { 
        get => _gameItem.Owner;
        set => _gameItem.Owner = value;
    }
}