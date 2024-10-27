using System;
using ContentLib.API.Model.Entity.Enemy;
using ContentLib.Core.Utils;

namespace ContentLib.EnemyAPI.Model.Enemy.Factories;
//TODO Might need a better more versatile approach. 
/// <summary>
/// Factory responsible for the creation of IEnemy instances from their respective properties. 
/// </summary>
/// <param name="enemyProperties">Properties of the enemy to create.</param>
public class EnemyFactory(IEnemyProperties enemyProperties) : IFactory<IEnemy>
{
    /// <summary>
    /// The properties of the enemy to make. 
    /// </summary>
    private IEnemyProperties _properties = enemyProperties;

    /// <inheritdoc />
    public IEnemy Create() => throw new NotImplementedException();
}