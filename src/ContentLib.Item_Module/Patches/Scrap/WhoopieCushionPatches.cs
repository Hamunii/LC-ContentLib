using ContentLib.API.Model.Event;
using ContentLib.API.Model.Item;
using ContentLib.API.Model.Item.Scrap;
using ContentLib.Core.Model.Managers;
using UnityEngine;


namespace ContentLib.Item_Module.Patches.Scrap;

public class WhoopieCushionFunctionalScrapPatches : BaseFunctionalScrapPatch<WhoopieCushionItem,IWhoopieCoushin>
{
    public static void Init()
    {
        BaseFunctionalScrapPatch<WhoopieCushionItem,IWhoopieCoushin>.Init<WhoopieCushionFunctionalScrapPatches>();
        On.WhoopieCushionItem.Fart += WhoopieCushionItemOnFart;
    }

    private static void WhoopieCushionItemOnFart(On.WhoopieCushionItem.orig_Fart orig, WhoopieCushionItem self)
    {
        if (ItemManager.Instance.GetItem(self.NetworkObjectId) is IWhoopieCoushin coushin)
        {
            ItemCollisionSoundEvent collisionSoundEvent = new WhoopieCusionFartEvent(coushin);
            GameEventManager.Instance.Trigger(collisionSoundEvent);
        }
            
        orig(self);
    }

    protected override IWhoopieCoushin CreateItem(WhoopieCushionItem instance)
    {
        return new WhoopieCoushionImpl(instance);
    }

    private class WhoopieCoushionImpl(WhoopieCushionItem whoopieCushionItem) : BaseScrapItem(whoopieCushionItem),IWhoopieCoushin
    {
        protected override string ScrapName => "Whoopie Cushion";
        public ScrapType Type => ScrapType.WhoopieCushion;

        public AudioClip[] FartClips
        {
            get => whoopieCushionItem.fartAudios;
            set => whoopieCushionItem.fartAudios = value;
        }
    }

    private class WhoopieCusionFartEvent(IWhoopieCoushin coushin) : ItemCollisionSoundEvent
    {
        public override Vector3 Position => coushin.Location;
        public override IGameItem? Item => coushin;

        public override AudioClip[]? PotentialCollisionSounds
        {
            get => coushin.FartClips;
            set => coushin.FartClips = value;
        }
    }

}