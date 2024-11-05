using System.Collections.Generic;
using ContentLib.API.Model.Item;
using ContentLib.Core.Utils;

namespace ContentLib.Item_Module.Model;

public class ItemManager
{
    public static ItemManager Instance { get; } = new();
    private Dictionary<ulong, IGameItem> _items;

    private ItemManager()
    {
        _items = new Dictionary<ulong, IGameItem>();
    }

    public void RegisterItem(IGameItem itemToRegister)
    
    { 
        _items.Add(itemToRegister.Id, itemToRegister);
        CLLogger.Instance.Log($"Registered item {itemToRegister.Id}");
    }
    public void UnRegisterItem(IGameItem itemToUnRegister) => _items.Remove(itemToUnRegister.Id);
    public void UnRegisterAllItems() => _items.Clear();
    public IGameItem GetItemById(ulong id) => _items[id];
}