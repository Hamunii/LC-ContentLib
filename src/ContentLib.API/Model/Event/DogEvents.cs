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
    public abstract Vector3 NoisePosition { get; }
    public abstract float NoiseLoudness { get; }
    public abstract int TimesNoisePlayedInOneSpot { get; }
    public abstract int NoiseID { get; }
}

/// <summary>
/// Abstract representation of an event when an Eyeless Dog alerts all other Dogs to their location
/// </summary>
public abstract class DogAlertEvent : IMonsterEvents
{
    public abstract bool IsCancelled { get; set; }
    public abstract IEnemy Enemy { get; }
    public abstract Vector3 AlertPosition { get; }
}

/// <summary>
/// Abstract representation of an event when an Eyeless Dog lunges 
/// </summary>
public abstract class DogLungeEvent : IMonsterEvents
{
    
    public abstract Quaternion LungeVector { get; }
    public abstract bool IsCancelled { get; set; }
    public abstract IEnemy Enemy { get; }
}

