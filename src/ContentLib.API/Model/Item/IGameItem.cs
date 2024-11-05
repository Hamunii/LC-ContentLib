using UnityEngine;

namespace ContentLib.API.Model.Item;
/// <summary>
/// Interface representing the general functionality of an in-game Item.
/// </summary>
public interface IGameItem
{
    ulong Id { get; }
    string Name { get;}
    float Weight { get; set; }
    bool IsOnShip { get; }
    Vector3 Location { get; }
}