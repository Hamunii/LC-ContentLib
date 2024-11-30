using System;
using ContentLib.API.Exceptions.Core.Manager;
using ContentLib.API.Model.Entity.Enemy.Vanilla.EyelessDog;
using ContentLib.API.Model.Event;
using ContentLib.Core.Utils;
using ContentLib.EnemyAPI.Events;
using ContentLib.EnemyAPI.Model.Enemy;
using ContentLib.entityAPI.Model.entity;
using UnityEngine;

namespace ContentLib.EnemyAPI.Patches;

/// <summary>
/// Class for patching the game with Eyeless Dog-related events and interactions.
/// </summary>
public class EyelessDogPatches
{
    /// <summary>
    /// Method to begin patching the game with Eyeless Dog related events and interactions
    /// </summary>
    public static void Init()
    {
        CLLogger.Instance.Log("Eyeless Dog being Patched");
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
    private static void MouthDogAIOnDetectNoise(On.MouthDogAI.orig_DetectNoise orig, MouthDogAI self,
        Vector3 noiseposition, float noiseloudness, int timesnoiseplayedinonespot, int noiseid)
    {
        bool noNoiseCooldown = (double)self.hearNoiseCooldown >= 0.0;
        orig(self, noiseposition, noiseloudness, timesnoiseplayedinonespot, noiseid);
        if ((double)self.stunNormalizedTimer > 0.0 || noiseid == 7 || noiseid == 546 || self.inKillAnimation ||
            noNoiseCooldown || timesnoiseplayedinonespot > 15){
            return;
        }
        
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
        
        IEnemy enemy = new LocalEyelessDog(self);
        try
        {
            EntityManager.Instance.RegisterEntity(enemy);
        }
        catch (InvalidEntityRegistrationException e)
        {
            CLLogger.Instance.DebugLog(e.ToString(), DebugLevel.EntityEvent);
        }
        
    }
    
    #region EyelessDogImplementation
    /// <summary>
    /// Class representing an Eyeless Dog for the purposes of patching
    /// </summary>
    /// <param name="mouthDogAI">The in-game reference to the Eyeless Dog</param>
    private class LocalEyelessDog(MouthDogAI mouthDogAI): IEyelessDog
    {
        /// <summary>
        /// The Network ID for this Eyeless Dog
        /// </summary>
        public ulong Id => mouthDogAI.NetworkObjectId;
        /// <summary>
        /// Boolean representing whether this Eyeless Dog is Alive
        /// </summary>
        public bool IsAlive => !mouthDogAI.isEnemyDead;
        /// <summary>
        /// Integer representing the health of this Eyeless Dog
        /// </summary>
        public int Health => mouthDogAI.enemyHP;
        /// <summary>
        /// The Vector 3 Position of this Eyeless Dog
        /// </summary>
        public Vector3 Position => mouthDogAI.gameObject.transform.position;
        // TODO: Left empty for now
        /// <summary>
        /// The Properties of this Eyeless Dog
        /// </summary>
        public IEnemyProperties EnemyProperties { get; }
        /// <summary>
        /// Boolean representing whether this Eyeless Dog has spawned
        /// </summary>
        public bool IsSpawned => mouthDogAI.IsSpawned;
        /// <summary>
        /// Boolean representing if this Entity is hostile (always True for Eyeless Dogs)
        /// </summary>
        public bool IsHostile => true;
        /// <summary>
        /// Boolean representing if this Eyeless Dog has entered Chase Mode
        /// </summary>
        public bool IsChasing => mouthDogAI.hasEnteredChaseModeFully;
        /// <summary>
        /// Method for immediately killing this Eyeless Dog
        /// </summary>
        public void Kill() => mouthDogAI.KillEnemy();
        /// <summary>
        /// Boolean representing if this Eyeless Dog is Lunging
        /// </summary>
        public bool isLunging => mouthDogAI.inLunge;
        /// <summary>
        /// Integer representing this Eyeless Dog's current Suspicion Level
        /// </summary>
        public int SuspicionLevel
        {
            get => mouthDogAI.suspicionLevel;
            set => mouthDogAI.suspicionLevel = value;
        }
        /// <summary>
        /// Vector3 Position representing the position this Eyeless Dog will head to when hearing a sound
        /// </summary>
        public Vector3 GuessedSearchPosition => mouthDogAI.noisePositionGuess;
        /// <summary>
        /// Vector3 Position representing the true position the sound this Eyeless Dog heard originated from
        /// </summary>
        public Vector3 AbsoluteSearchPosition => mouthDogAI.lastHeardNoisePosition;
        /// <summary>
        /// Method that causes this Eyeless Dog to Lunge toward its current heading
        /// </summary>
        public void Lunge() => mouthDogAI.EnterLunge();
        /// <summary>
        /// Method that causes this Eyeless Dog to grab a given Dead Body into its mouth, used when it kills a player
        /// </summary>
        /// <param name="body">A dead Player body</param>
        public void GrabDeadBody(DeadBodyInfo body) => mouthDogAI.TakeBodyInMouth(body);
        /// <summary>
        /// Method that causes this Eyeless Dog to drop any dead body it's holding in its mouth
        /// </summary>
        public void DropDeadBody() => mouthDogAI.DropCarriedBody();
        /// <summary>
        /// Method that causes this Eyeless Dog to howl into the air, calling all other Eyeless Dogs to their location
        /// </summary>
        public void AlertOtherDogs() => mouthDogAI.CallAllDogsWithHowl();
        /// <summary>
        /// Method that causes this Eyeless Dog to "hear" a sound with given parameters
        /// </summary>
        /// <param name="noisePosition">The Vector3 position of the sound</param>
        /// <param name="noiseLoudness">Float representing the loudness of the sound</param>
        /// <param name="timesNoisePlayedInOneSpot">Integer for how many times this sound was played in the same spot</param>
        /// <param name="noiseID">Integer representing the ID of the sound</param>
        public void DetectNoise(Vector3 noisePosition, float noiseLoudness, int timesNoisePlayedInOneSpot = 0, int noiseID = 0) => mouthDogAI.DetectNoise(noisePosition, noiseLoudness, timesNoisePlayedInOneSpot, noiseID);
        
        /// <summary>
        /// Method that causes this Eyeless Dog to immediately head to the given location as if it heard another Eyeless Dog howl
        /// </summary>
        /// <param name="howlPosition">The Vector3 position of the other Eyeless Dog's howl</param>
        public void ReactToDogAlert(Vector3 howlPosition) => mouthDogAI.ReactToOtherDogHowl(howlPosition);
        
        // TODO: Fix discrepancy as Enemies have their own Kill method
        /// <summary>
        /// Kills this entity as if it were a player (this method is unnecessary and is due for fixing).
        /// </summary>
        /// <param name="bodyVelocity"></param>
        /// <param name="spawnBody"></param>
        /// <param name="causeOfDeath"></param>
        /// <param name="deathAnimation"></param>
        /// <param name="positionOffset"></param>
        public void Kill(Vector3 bodyVelocity, bool spawnBody = true, CauseOfDeath causeOfDeath = CauseOfDeath.Unknown,
            int deathAnimation = 0, Vector3 positionOffset = default(Vector3)) =>
            throw new System.NotImplementedException();
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
        public override IEnemy Enemy => (IEnemy) EntityManager.Instance.GetEntity(mouthDogAI.NetworkObjectId);
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
        public override IEnemy Enemy => (IEnemy) EntityManager.Instance.GetEntity(mouthDogAI.NetworkObjectId);
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
        public override IEnemy Enemy => (IEnemy) EntityManager.Instance.GetEntity(mouthDogAI.NetworkObjectId);
        public override Vector3 AlertPosition => mouthDogAI.transform.position;
    }
    #endregion
}