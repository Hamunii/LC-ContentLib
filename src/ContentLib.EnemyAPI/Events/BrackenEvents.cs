using ContentLib.EnemyAPI.Model.Enemy;

namespace ContentLib.EnemyAPI.Events;

public abstract class BrackenSeenByPlayerEvent : IMonsterEvents
{
    /// <inheritdoc />
    public abstract IEnemy Enemy { get; }

    public bool IsCancelled { get; set; }
}