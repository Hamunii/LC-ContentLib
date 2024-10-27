using System;
using ContentLib.API.Model.Entity.Enemy;
using ContentLib.Core.Utils;
using ContentLib.EnemyAPI.Model.Enemy;
using UnityEngine;

namespace ContentLib.EnemyAPI.Model.Enemy.Custom
{
    /// <summary>
    /// Factory responsible for creating custom EnemyType from IEnemyProperties instances.
    /// </summary>
    public class CustomEnemyFactory : IFactory<EnemyType>
    {
        private readonly IEnemyProperties _properties;
        private readonly IEnemyHordeProperties _hordeProperties;

        public CustomEnemyFactory(IEnemy enemy)
        {
            _properties = enemy.EnemyProperties;
            
        }

        public EnemyType Create()
        {
            EnemyTypeBuilder builder = new EnemyTypeBuilder();
            SetEnemyProperties(builder);
            if (_hordeProperties != null)
            {
                SetHordeProperties(builder);
            }
            else
            {
                SetDefaultHordeProperties(builder);
            }
            return builder.Build();
        }

        private EnemyTypeBuilder SetEnemyProperties(EnemyTypeBuilder builder)
        {
            return builder
                .SetEnemyName(_properties.Name)
                .SetSpawningDisabled(_properties.SpawningDisabled)
                .SetIsOutsideEnemy(_properties.IsOutsideEnemy)
                .SetIsDaytimeEnemy(_properties.IsDaytimeEnemy)
                .SetSpawnFromWeeds(_properties.SpawnFromWeeds)
                .SetProbabilityCurve(_properties.ProbabilityCurve)
                .SetEnemyPrefab(_properties.EnemyPrefab)
                .SetPowerLevel(_properties.PowerLevel)
                .SetMaxCount(_properties.MaxCount)
                .SetCanBeStunned(_properties.CanBeStunned)
                .SetCanDie(_properties.CanDie)
                .SetDestroyOnDeath(_properties.DestroyOnDeath)
                .SetStunTimeMultiplier(_properties.StunTimeMultiplier)
                .SetDoorSpeedMultiplier(_properties.DoorSpeedMultiplier)
                .SetStunGameDifficultyMultiplier(_properties.StunGameDifficultyMultiplier)
                .SetCanSeeThroughFog(_properties.CanSeeThroughFog)
                .SetPushPlayerForce(_properties.PushPlayerForce)
                .SetPushPlayerDistance(_properties.PushPlayerDistance)
                .SetHitBodySFX(_properties.HitBodySFX)
                .SetHitEnemyVoiceSFX(_properties.HitEnemyVoiceSFX)
                .SetDeathSFX(_properties.DeathSFX)
                .SetStunSFX(_properties.StunSFX)
                .SetMiscAnimations(_properties.MiscAnimations)
                .SetAudioClips(_properties.AudioClips)
                .SetTimeToPlayAudio(_properties.TimeToPlayAudio)
                .SetLoudnessMultiplier(_properties.LoudnessMultiplier)
                .SetOverrideVentSFX(_properties.OverrideVentSFX);
        }

        private EnemyTypeBuilder SetHordeProperties(EnemyTypeBuilder builder)
        {
            return builder

                .SetNumberSpawnedFalloff(_hordeProperties.NumberSpawnedFalloff, _hordeProperties.UseNumberSpawnedFalloff)
                .SetSpawnInGroupsOf(_hordeProperties.SpawnInGroupsOf)
                .SetRequireNestObjectsToSpawn(_hordeProperties.RequireNestObjectsToSpawn)
                .SetNormalizedTimeInDayToLeave(_hordeProperties.NormalizedTimeInDayToLeave)
                .SetSizeLimit(_hordeProperties.SizeLimit)
                .SetNestSpawnPrefab(_hordeProperties.NestSpawnPrefab)
                .SetNestSpawnPrefabWidth(_hordeProperties.NestSpawnPrefabWidth)
                .SetUseMinEnemyThresholdForNest(_hordeProperties.UseMinEnemyThresholdForNest)
                .SetMinEnemiesToSpawnNest(_hordeProperties.MinEnemiesToSpawnNest);
        }

        private EnemyTypeBuilder SetDefaultHordeProperties(EnemyTypeBuilder builder)
        {
            return builder
                .SetNumberSpawnedFalloff(null, false)
                .SetSpawnInGroupsOf(1)
                .SetRequireNestObjectsToSpawn(false)
                .SetNormalizedTimeInDayToLeave(0f)
                .SetSizeLimit(NavSizeLimit.NoLimit);
        }
    }
}
