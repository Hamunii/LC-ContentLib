namespace ContentLib.API.Model.Entity.Util;

/// <summary>
/// Interface representing an instance that is able to be "Healed". This can be both to represent literal "Health" of
/// something like an IGameEntity, or something physical, such as the Health of a Veichle. 
/// </summary>
public interface IHealable
{
    /// <summary>
    /// Heal the instance for the specified amount, over the given amount of seconds. 
    /// </summary>
    /// <param name="healAmount">The amount to heal the instance by.</param>
    /// <param name="healDurationInSeconds">The amount of time it takes to fully heal the instance to the given amount,
    /// in seconds.</param>>
    /// 
    void HealOverTime(int healAmount, float healDurationInSeconds);

    /// <summary>
    /// Heal the instance for the specified amount, instantly. 
    /// </summary>
    /// <param name="healAmount">The amount to heal the instance by.</param>
    void Heal(int healAmount);
}