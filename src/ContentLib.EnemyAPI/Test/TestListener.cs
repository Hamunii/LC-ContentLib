using ContentLib.Core.Model.Event;
using ContentLib.Core.Model.Event.Listener;
using ContentLib.EnemyAPI.Events;
using ContentLib.EnemyAPI.Model.Enemy;
using ContentLib.EnemyAPI.Model.Enemy.Vanilla.Bracken;
using UnityEngine;

namespace ContentLib.EnemyAPI.Test;

public class TestListener : IListener
{
    [EventDelegate]
    private void OnMonsterKill(MonsterKillsPlayerEvent killsPlayerEvent)
    {
        IEnemy enemy = killsPlayerEvent.Enemy;
        if (enemy is IBracken bracken)
        {
            Debug.Log($"[$LC-ContentLib] The player has been killed by a Braken with id: {bracken.Id}");
        }
    }

    [EventDelegate]
    private void OnMonsterSpawn(PlayerSpawnEvent playerSpawnEvent)
    {
        playerSpawnEvent.Player.TeleportToShip();
    }
}