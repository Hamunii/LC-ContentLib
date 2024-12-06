using ContentLib.API.Model.Entity;
using ContentLib.API.Model.Item;
using ContentLib.API.Model.Item.Scrap;
using ContentLib.Core.Utils;
using UnityEngine;

namespace ContentLib.Item_Module.Patches.Scrap
{
    public class RemoteFunctionalScrapPatches : BaseFunctionalScrapPatch<RemoteProp, IRemoteControlScrap>
    {
        public static void Init() => BaseFunctionalScrapPatch<RemoteProp, IRemoteControlScrap>.Init<RemoteFunctionalScrapPatches>();

        protected override IRemoteControlScrap CreateItem(RemoteProp instance) => new RemoteControlImpl(instance);

        private class RemoteControlImpl(GrabbableObject remoteObject) : BaseScrapItem(remoteObject), IRemoteControlScrap
        {
            protected override string ScrapName => "Remote Control";
            public ScrapType Type => ScrapType.Remote;
        }
    }
}