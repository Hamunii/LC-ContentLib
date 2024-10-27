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
        public IEnemyProperties EnemyProperties { get; }
        public bool IsSpawned => mouthDogAI.IsSpawned;
        public bool IsHostile { get; }
        public bool IsChasing { get; }
        public void Kill() => throw new System.NotImplementedException();
        public bool isLunging { get; }
        public int suspicionLevel { get; }
        public Vector3 GuessedSearchPosition { get; }
        public Vector3 AbsoluteSearchPosition { get; }
        public void Lunge() => throw new System.NotImplementedException();

        public void GrabDeadBody(DeadBodyInfo body) => throw new System.NotImplementedException();

        public void DropDeadBody() => throw new System.NotImplementedException();

        public void AlertOtherDogs() => throw new System.NotImplementedException();
    }
}