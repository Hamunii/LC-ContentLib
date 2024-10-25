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
        public bool IsAlive { get; }
        public int Health { get; }
        public Vector3 Position { get; }
        public IEnemyProperties EnemyProperties { get; }
        public bool IsSpawned { get; }
        public bool IsHostile { get; }
        public bool IsChasing { get; }
    }
}