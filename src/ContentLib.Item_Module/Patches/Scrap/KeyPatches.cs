using ContentLib.API.Model.Item.Scrap;

namespace ContentLib.Item_Module.Patches.Scrap;

public class KeyFunctionalScrapPatches : BaseFunctionalScrapPatch<KeyItem,IKey>
{
    public static void Init() => BaseFunctionalScrapPatch<KeyItem,IKey>.Init<KeyFunctionalScrapPatches>();

    protected override IKey CreateItem(KeyItem instance) => new KeyImpl(instance);

    private class KeyImpl(KeyItem instance) : BaseScrapItem(instance), IKey
    {
        protected override string ScrapName => "Key";
        public ScrapType Type => ScrapType.Key;
    }
}