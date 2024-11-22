using ContentLib.API.Model.Entity;
using ContentLib.API.Model.Item;
using ContentLib.Core.Utils;
using UnityEngine;

namespace ContentLib.Item_Module.Patches.Scrap;

public class WhoopieCushionPatches : BasePatch<WhoopieCushionItem,IWhoopieCoushin>
{
    public static void Init()
    {
        BasePatch<WhoopieCushionItem,IWhoopieCoushin>.Init<WhoopieCushionPatches>();
    }
    protected override IWhoopieCoushin CreateItem(WhoopieCushionItem instance)
    {
        return new WhoopieCoushionImpl(instance);
    }

    private class WhoopieCoushionImpl(WhoopieCushionItem whoopieCushionItem) : BaseScrapItem(whoopieCushionItem),IWhoopieCoushin
    {
        protected override string ScrapName => "Whoopie Cushion";
    }
    
}