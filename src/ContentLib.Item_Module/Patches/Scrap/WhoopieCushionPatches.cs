using ContentLib.API.Model.Item.Scrap;


namespace ContentLib.Item_Module.Patches.Scrap;

public class WhoopieCushionFunctionalScrapPatches : BaseFunctionalScrapPatch<WhoopieCushionItem,IWhoopieCoushin>
{
    public static void Init()
    {
        BaseFunctionalScrapPatch<WhoopieCushionItem,IWhoopieCoushin>.Init<WhoopieCushionFunctionalScrapPatches>();
    }
    protected override IWhoopieCoushin CreateItem(WhoopieCushionItem instance)
    {
        return new WhoopieCoushionImpl(instance);
    }

    private class WhoopieCoushionImpl(WhoopieCushionItem whoopieCushionItem) : BaseScrapItem(whoopieCushionItem),IWhoopieCoushin
    {
        protected override string ScrapName => "Whoopie Cushion";
        public ScrapType Type => ScrapType.WhoopieCushion;
    }
    
}