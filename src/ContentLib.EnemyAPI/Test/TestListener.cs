using ContentLib.API.Model.Entity.Enemy;
using ContentLib.API.Model.Entity.Enemy.Vanilla.OldBird;
using ContentLib.Core.Model.Event;
using ContentLib.Core.Model.Event.Listener;
using ContentLib.Core.Utils;
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
            CLLogger.Instance.Log($"[$LC-ContentLib] The player has been killed by a Braken with id: {bracken.Id}");
        }
    }

    [EventDelegate]
    private void OnMonsterWakeUp(WakeableMonsterWakeEvent wakeUpEvent)
    {
        if (wakeUpEvent.Enemy is IOldBird)
        {
            CLLogger.Instance.Log("An Old Bird Woke up!");
        }
    }
}