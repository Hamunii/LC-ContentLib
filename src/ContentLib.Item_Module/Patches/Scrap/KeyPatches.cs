using ContentLib.API.Model.Item;
using IL;
using UnityEngine;

namespace ContentLib.Item_Module.Patches.Scrap;

public class KeyPatches : BasePatch<KeyItem,IKey>
{
    public static void Init() => BasePatch<KeyItem,IKey>.Init<KeyPatches>();

    protected override IKey CreateItem(KeyItem instance) => new KeyImpl(instance);

    private class KeyImpl(KeyItem instance) : BaseScrapItem(instance), IKey
    {
        protected override string ScrapName => "Key";
    }
}