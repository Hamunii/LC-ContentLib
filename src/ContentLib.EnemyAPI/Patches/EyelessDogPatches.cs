using ContentLib.API.Model.Entity.Enemy.Vanilla.EyelessDog;
using ContentLib.API.Model.Event;
using ContentLib.EnemyAPI.Events;
using ContentLib.EnemyAPI.Model.Enemy;
using UnityEngine;

namespace ContentLib.EnemyAPI.Patches;

public class EyelessDogPatches
{
    public static void Init()
    {
        On.MouthDogAI.Start += MouthDogAIOnStart;
        On.MouthDogAI.EnterLunge += MouthDogAIOnEnterLunge;
        On.MouthDogAI.DetectNoise += MouthDogAIOnDetectNoise;
        On.MouthDogAI.CallAllDogsWithHowl += MouthDogAIOnCallAllDogsWithHowl;
    }
    
    #region EventTriggers
    /// <summary>
    /// Patch for triggering a DogAlertEvent when an Eyeless Dog alerts other Dogs
    /// </summary>
    /// <param name="orig">The original method being executed</param>
    /// <param name="self">The Eyeless Dog executing the method</param>
    private static void MouthDogAIOnCallAllDogsWithHowl(On.MouthDogAI.orig_CallAllDogsWithHowl orig, MouthDogAI self)
    {
        orig(self);
        GameEventManager.Instance.Trigger(new DogAlertEventImp(self));
    }
    /// <summary>
    /// Patch for triggering a DogHearsNoiseEvent whenever an Eyeless Dog hears a noise
    /// </summary>
    /// <param name="orig">The original method being executed</param>
    /// <param name="self">the Eyeless Dog executing the method</param>
    /// <param name="noiseposition">The position of the noise heard</param>
    /// <param name="noiseloudness">The loudness of the noise heard</param>
    /// <param name="timesnoiseplayedinonespot">The times a noise has been played in the same spot</param>
    /// <param name="noiseid">The type of noise heard</param>
    private static void MouthDogAIOnDetectNoise(On.MouthDogAI.orig_DetectNoise orig, MouthDogAI self, Vector3 noiseposition, float noiseloudness, int timesnoiseplayedinonespot, int noiseid)
    {
        orig(self, noiseposition, noiseloudness, timesnoiseplayedinonespot, noiseid);
        GameEventManager.Instance.Trigger(new DogHearsNoiseEventImp(self, noiseposition, noiseloudness,
            timesnoiseplayedinonespot, noiseid));
    }

    /// <summary>
    /// Patch for triggering a DogLungeEvent whenever an Eyeless Dog lunges
    /// </summary>
    /// <param name="orig">The original method being executed</param>
    /// <param name="self">The Eyeless Dog executing the method</param>
    private static void MouthDogAIOnEnterLunge(On.MouthDogAI.orig_EnterLunge orig, MouthDogAI self)
    {
        orig(self);
        GameEventManager.Instance.Trigger(new DogLungeEventImp(self));
    }
    
    #endregion
    
    /// <summary>
    /// Patch for registering an Eyeless Dog when one spawns
    /// </summary>
    /// <param name="orig">The method being executed</param>
    /// <param name="self">The Eyeless Dog being spawned</param>
    private static void MouthDogAIOnStart(On.MouthDogAI.orig_Start orig, MouthDogAI self)
    {
        orig(self);
        Debug.Log("Eyeless Dog Patches!");
        IEnemy enemy = new LocalEyelessDog(self);
        EnemyManager.Instance.RegisterEnemy(enemy);
    }
    
    #region EyelessDogImplementation
    /// <summary>
    /// Class representing an Eyeless Dog for the purposes of patching
    /// </summary>
    /// <param name="mouthDogAI">The in-game reference to the Eyeless Dog</param>
    private class LocalEyelessDog(MouthDogAI mouthDogAI): IEyelessDog
    {
        public ulong Id => mouthDogAI.NetworkObjectId;
        public bool IsAlive => !mouthDogAI.isEnemyDead;
        public int Health => mouthDogAI.enemyHP;
        public Vector3 Position => mouthDogAI.gameObject.transform.position;
        // TODO: Left empty for now
        public IEnemyProperties EnemyProperties { get; }
        public bool IsSpawned => mouthDogAI.IsSpawned;
        public bool IsHostile => true;
        public bool IsChasing => mouthDogAI.hasEnteredChaseModeFully;
        public void Kill() => mouthDogAI.KillEnemy();
        public bool isLunging => mouthDogAI.inLunge;
        public int SuspicionLevel
        {
            get => mouthDogAI.suspicionLevel;
            set => mouthDogAI.suspicionLevel = value;
        }
        public Vector3 GuessedSearchPosition => mouthDogAI.noisePositionGuess;
        public Vector3 AbsoluteSearchPosition => mouthDogAI.lastHeardNoisePosition;
        public void Lunge() => mouthDogAI.EnterLunge();
        public void GrabDeadBody(DeadBodyInfo body) => mouthDogAI.TakeBodyInMouth(body);
        public void DropDeadBody() => mouthDogAI.DropCarriedBody();
        public void AlertOtherDogs() => mouthDogAI.CallAllDogsWithHowl();
        public void DetectNoise(Vector3 noisePosition, float noiseLoudness, int timesNoisePlayedInOneSpot = 0, int noiseID = 0) => mouthDogAI.DetectNoise(noisePosition, noiseLoudness, timesNoisePlayedInOneSpot, noiseID);

        public void ReactToDogAlert(Vector3 howlPosition) => mouthDogAI.ReactToOtherDogHowl(howlPosition);
    }
    #endregion
    
    #region DogEventImplementations
    /// <summary>
    /// Event referencing when an Eyeless Dog lunges toward a location
    /// </summary>
    /// <param name="mouthDogAI">The Eyeless Dog lunging</param>
    private class DogLungeEventImp(MouthDogAI mouthDogAI) : DogLungeEvent
    {
        public override Quaternion LungeVector => mouthDogAI.transform.rotation;
        public override bool IsCancelled { get; set; }
        public override IEnemy Enemy => EnemyManager.Instance.GetEnemy(mouthDogAI.NetworkObjectId);
    }
    
    /// <summary>
    /// Event referencing when an Eyeless Dog hears a noise
    /// </summary>
    /// <param name="mouthDogAI">The Eyeless Dog that heard the noise</param>
    /// <param name="noisePosition">The position of the noise</param>
    /// <param name="noiseLoudness">The volume of the noise</param>
    /// <param name="timesNoisePlayed">The times that same sound played in the same location</param>
    /// <param name="noiseID">The type of noise heard</param>
    private class DogHearsNoiseEventImp(
        MouthDogAI mouthDogAI,
        Vector3 noisePosition,
        float noiseLoudness,
        int timesNoisePlayed,
        int noiseID) : DogHearsNoiseEvent
    {
        public override bool IsCancelled { get; set; }
        public override IEnemy Enemy => EnemyManager.Instance.GetEnemy(mouthDogAI.NetworkObjectId);
        public override Vector3 NoisePosition => noisePosition;
        public override float NoiseLoudness => noiseLoudness;
        public override int TimesNoisePlayedInOneSpot => timesNoisePlayed;
        public override int NoiseID => noiseID;
    }

    /// <summary>
    /// Event referencing when an Eyeless Dog howls, alerting all other Dogs on the map
    /// </summary>
    /// <param name="mouthDogAI">The Eyeless Dog causing the alert</param>
    private class DogAlertEventImp(MouthDogAI mouthDogAI) : DogAlertEvent
    {
        public override bool IsCancelled { get; set; }
        public override IEnemy Enemy => EnemyManager.Instance.GetEnemy(mouthDogAI.NetworkObjectId);
        public override Vector3 AlertPosition => mouthDogAI.transform.position;
    }
    #endregion
}