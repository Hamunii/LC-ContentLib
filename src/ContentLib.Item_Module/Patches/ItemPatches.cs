using ContentLib.API.Model.Item;

namespace ContentLib.Item_Module.Patches;

public class ItemPatches
{
    public static void Init()
    {
        On.RemoteProp.ItemActivate += RemotePropOnItemActivate;
    }

    private static void RemotePropOnItemActivate(On.RemoteProp.orig_ItemActivate orig, RemoteProp self, bool used, bool buttondown)
    {
        throw new System.NotImplementedException();
    }

    private class RemoteItem(RemoteProp prop) : IGameItem
    {
        
    }
}