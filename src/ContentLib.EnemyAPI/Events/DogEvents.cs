using ContentLib.API.Model.Entity.Enemy.Vanilla.EyelessDog;
using ContentLib.Core.Model.Event;
using ContentLib.EnemyAPI.Model.Enemy;
using UnityEngine;

namespace ContentLib.EnemyAPI.Events;

public abstract class DogHearsNoiseEvent : IMonsterEvents
{
    public bool IsCancelled { get; set; }
    public IEnemy Enemy { get; }
    public Vector3 NoisePosition { get; }
    public float NoiseLoudness { get; }
    public int TimesNoisePlayedInOneSpot { get; }
    public int NoiseID { get; }
}

public abstract class DogHearsAlertEvent : IMonsterEvents
{
    public bool IsCancelled { get; set; }
    public IEnemy Enemy { get; }
    public Vector3 AlertPosition { get; }
    public IEyelessDog AlertingDog { get; }
}

public abstract class DogLungeEvent : IMonsterEvents
{
    public bool IsCancelled { get; set; }
    public IEnemy Enemy { get; }
    public Quaternion LungeVector { get; }
}

