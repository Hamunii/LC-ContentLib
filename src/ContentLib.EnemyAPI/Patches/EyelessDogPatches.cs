using ContentLib.API.Model.Entity.Enemy.Vanilla.EyelessDog;
using ContentLib.EnemyAPI.Model.Enemy;
using UnityEngine;

namespace ContentLib.EnemyAPI.Patches;

public class EyelessDogPatches
{
    public static void Init()
    {
        On.MouthDogAI.Start += MouthDogAIOnStart;
    }

    private static void MouthDogAIOnStart(On.MouthDogAI.orig_Start orig, MouthDogAI self)
    {
        orig(self);
        Debug.Log("Eyeless Dog Patches!");
        IEnemy enemy = new LocalEyelessDog(self);
        EnemyManager.Instance.RegisterEnemy(enemy);
    }

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
        public int SuspicionLevel => mouthDogAI.suspicionLevel;
        public Vector3 GuessedSearchPosition => mouthDogAI.noisePositionGuess;
        public Vector3 AbsoluteSearchPosition => mouthDogAI.lastHeardNoisePosition;
        public void Lunge() => mouthDogAI.EnterLunge();
        public void GrabDeadBody(DeadBodyInfo body) => mouthDogAI.TakeBodyInMouth(body);
        public void DropDeadBody() => mouthDogAI.DropCarriedBody();
        public void AlertOtherDogs() => mouthDogAI.CallAllDogsWithHowl();
        public void DetectNoise(Vector3 noisePosition, float noiseLoudness, int timesNoisePlayedInOneSpot = 0, int noiseID = 0) => mouthDogAI.DetectNoise(noisePosition, noiseLoudness, timesNoisePlayedInOneSpot, noiseID);

        public void ReactToOtherDogHowl(Vector3 howlPosition) => mouthDogAI.ReactToOtherDogHowl(howlPosition);
    }
}