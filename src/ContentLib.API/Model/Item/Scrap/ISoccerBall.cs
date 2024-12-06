using UnityEngine;

namespace ContentLib.API.Model.Item.Scrap;

/// <summary>
/// Interface representing the general functionality of a Soccer Ball Scrap item.
/// </summary>
public interface ISoccerBall : IVanillaScrap
{
    /// <summary>
    /// The current velocity of the ball (post-kick).
    /// </summary>
    Vector3 Velocity { get; }
}

