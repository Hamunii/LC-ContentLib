using ContentLib.API.Model.Entity.Enemy.Vanilla.EyelessDog;
using ContentLib.API.Model.Event;
using ContentLib.Core.Model.Event;
using ContentLib.EnemyAPI.Model.Enemy;
using UnityEngine;

namespace ContentLib.EnemyAPI.Events;

/// <summary>
/// Abstract representation of an event when an Eyeless Dog hears a noise
/// </summary>
public abstract class DogHearsNoiseEvent : IMonsterEvents
{
    public abstract bool IsCancelled { get; set; }
    public abstract IEnemy Enemy { get; }
    /// <summary>
    /// The Vector3 position of the Noise the Eyeless Dog heard
    /// </summary>
    public abstract Vector3 NoisePosition { get; }
    /// <summary>
    /// Float representing the Loudness of the noise the Eyeless Dog heard
    /// </summary>
    public abstract float NoiseLoudness { get; }
    /// <summary>
    /// Integer representing the amount of times a noise played in the same spot
    /// </summary>
    public abstract int TimesNoisePlayedInOneSpot { get; }
    /// <summary>
    /// The Integer ID of the noise the Eyeless Dog heard
    /// </summary>
    public abstract int NoiseID { get; }
}

/// <summary>
/// Abstract representation of an event when an Eyeless Dog alerts all other Dogs to their location
/// </summary>
public abstract class DogAlertEvent : IMonsterEvents
{
    public abstract bool IsCancelled { get; set; }
    public abstract IEnemy Enemy { get; }
    /// <summary>
    /// The Vector3 position where other Eyeless Dogs will head toward
    /// </summary>
    public abstract Vector3 AlertPosition { get; }
}

/// <summary>
/// Abstract representation of an event when an Eyeless Dog lunges 
/// </summary>
public abstract class DogLungeEvent : IMonsterEvents
{
    /// <summary>
    /// The Quaternion vector this EyelessDog is lunging toward
    /// </summary>
    public abstract Quaternion LungeVector { get; }
    public abstract bool IsCancelled { get; set; }
    public abstract IEnemy Enemy { get; }
}

