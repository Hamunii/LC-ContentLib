using System;
using System.Collections.Generic;
using GameNetcodeStuff;

namespace ContentLib.EnemyAPI;

public class PlayerManager
{
    /// <summary>
    /// Singleton pattern call via a lazy implementation of the manager.
    /// </summary>
    private static readonly Lazy<PlayerManager> instance = new Lazy<PlayerManager>(() => new PlayerManager());

    /// <summary>
    /// Gets the singleton instance of the manager.
    /// </summary>
    public static PlayerManager Instance => instance.Value;
    /// <summary>
    /// Dictionary of PlayerControllerB values, identified by their NetworkObjectId keys. Allows location of a Player
    /// via their NetworkObjectId. 
    /// </summary>
    private Dictionary<ulong, PlayerControllerB> playersCache  = new();

    public void RegisterPlayer(ulong id, PlayerControllerB player)
    {
        playersCache.Add(id, player);
    }

    public void UnregisterPlayer(ulong id)
    {
        playersCache.Remove(id);
    }

    public PlayerControllerB? GetPlayer(ulong id)
    {
        return playersCache[id];
    }

}