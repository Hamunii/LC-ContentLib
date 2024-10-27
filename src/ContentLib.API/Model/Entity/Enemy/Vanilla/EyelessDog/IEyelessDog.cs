using ContentLib.Core.Model.Entity.Util;
using ContentLib.EnemyAPI.Model.Enemy;
using UnityEngine;

namespace ContentLib.API.Model.Entity.Enemy.Vanilla.EyelessDog;

/// <summary>
/// Interface representing the general functionality of the Eyeless Dog Vanilla Enemy. 
/// </summary>
public interface IEyelessDog : IEnemy, IKillable
{
    #region Fields

    /// <summary>
    /// Boolean representing whether this Eyeless Dog is lunging or not.
    /// </summary>
    bool isLunging { get; }
    int suspicionLevel { get; }
    Vector3 GuessedSearchPosition { get; }
    Vector3 AbsoluteSearchPosition { get; }


    #endregion
    
    #region actions

    void Lunge();
    void GrabDeadBody(DeadBodyInfo body);
    void DropDeadBody();

    void AlertOtherDogs();


    #endregion

    #region suspicion

    #endregion
}