using ContentLib.API.Model.Entity.Enemy;
using ContentLib.Core.Model.Entity.Util;
using Unity.Netcode;

namespace ContentLib.EnemyAPI.Model.Enemy.Vanilla.Bracken;

/// <summary>
/// Interface representing the general functionality of a Bracken.
/// </summary>
public interface IBracken : IEnemy, IKillable
{
    /// <summary>
    /// Adds a set amount to the Bracken's anger meter. 
    /// </summary>
    /// <param name="amountToAdd">The amount to add to the Bracken Meter.</param>
    void AddToAngerMeter(float amountToAdd);
    
    /// <summary>
    /// The current anger of the Bracken.
    /// </summary>
    float Anger { get; }
    
    /// <summary>
    /// Checks the curent anger state of the Bracken.
    /// </summary>
    bool IsAngry { get; }

    /// <summary>
    /// Sends a ServerRPC to send the Bracken into Anger mode.
    /// </summary>
    /// <param name="angerTime">The amount of time to be angry for.</param>
    [ServerRpc]
    void EnterAngerModeServerRpc(float angerTime);

    /// <summary>
    /// Sends a client RPC to send the Bracken into Anger mode.
    /// </summary>
    /// <param name="angerTime">The amount of time to be angry for.</param>
    [ClientRpc]
    void EnterAngerModeClientRpc(float angerTime);
    
    /// <summary>
    /// Tells the Bracken to a avoid the closest player.
    /// </summary>
    void AvoidClosestPlayer();
    
    /// <summary>
    /// Method to force the Bracken to look at a player who has triggered its observation behaviour. 
    /// </summary>
    /// <param name="playerId">The id of the player.</param>
    void LookAtTrigger(int playerId);
    
    //TODO more context required in the docs here.
    /// <summary>
    /// Sends a Server RPC to reset the stealth timer of the Bracken.
    /// </summary>
    /// <param name="playerObj">TBD</param>
    [ServerRpc]
    void ResetStealthTimerServerRpc(int playerObj);
    
    //TODO more context required in the docs here.
    /// <summary>
    /// Sends a Client RPC to reset the stealth timer of the Bracken.
    /// </summary>
    /// <param name="playerObj">TBD</param>
    [ClientRpc]
    void ResetStealthTimerClientRpc(int playerObj);
    
}