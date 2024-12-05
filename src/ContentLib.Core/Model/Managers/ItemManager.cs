using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices.Model.Managers;
using ContentLib.API.Exceptions.Core.Manager;
using ContentLib.API.Model.Item;
using ContentLib.Core.Utils;

namespace ContentLib.Core.Model.Managers;

/// <summary>
/// Manager responsible for the registration and handling of IGameItems. 
/// </summary>
public class ItemManager : IItemManager
{
    /// <summary>
    /// Returns , or creates the singleton instance of the Item Manager. 
    /// </summary>
    /// <returns>The singleton instance.</returns>
    public static ItemManager Instance { get; } = new();

    /// <summary>
    /// Dictionary of every registered item within the gameworld. 
    /// </summary>
    private Dictionary<ulong, IGameItem> _items;

    /// <summary>
    /// Private constructor that initialises the _items Dictionary. <i>(Developer Note: Keep private to ensure there
    /// is no way to construct this manager other than the singleton getter.)</i>
    /// </summary>
    private ItemManager()
    {
        _items = new Dictionary<ulong, IGameItem>();
    }

    /// <summary>
    /// Registers an item to the manager, allowing for it to be managed via the api during game.
    /// </summary>
    /// <param name="itemToRegister">The item to register.</param>
    public void RegisterItem(IGameItem itemToRegister)

    {
        try
        {
            _items.Add(itemToRegister.Id, itemToRegister);
            CLLogger.Instance.DebugLog($"Registered item {itemToRegister.Id}", DebugLevel.ItemEvent);
        }
        catch (Exception exception)
        {
            throw new InvalidItemRegistrationException(itemToRegister, exception);
        }
       
    }

    /// <summary>
    /// Unregisters an item from the manager, not typically called, but might have unique circumstances, such as a mod
    /// that destroys items. 
    /// </summary>
    /// <param name="id">The id of the item to unregister.</param>
    public void UnRegisterItem(ulong id) => _items.Remove(id);

    /// <summary>
    /// Unregisters all the items from the manager, typically called at the end of a session.
    /// </summary>
    public void UnRegisterAllItems() => _items.Clear();

    /// <summary>
    /// Unregisters all the non-persisting items (i.e. items left on the moon after leaving).
    /// </summary>
    public void UnRegisterNonPersistingItems()
    {
        CLLogger.Instance.Log("Unregistering non-persisting items");
        foreach (var keyValuePair in _items)
        {
            if (!keyValuePair.Value.IsOnShip)
                _items.Remove(keyValuePair.Key);
        }
    }

    /// <summary>
    /// Gets an item with the given id. 
    /// </summary>
    /// <param name="id">The id of the item to get</param>
    /// <returns>The item with the given id</returns>
    public IGameItem? GetItem(ulong id)
    {
        if (_items.TryGetValue(id, out var item))
        {
            return item;
        }

        return null;
    }
}

