using ContentLib.API.Model.Entity;
using ContentLib.API.Model.Item;
using ContentLib.Core.Utils;
using UnityEngine;

namespace ContentLib.Item_Module.Patches.Scrap
{
    public class RemotePatches : BasePatch<RemoteProp, IRemoteControlScrap>
    {
        public static void Init() => BasePatch<RemoteProp, IRemoteControlScrap>.Init<RemotePatches>();

        protected override IRemoteControlScrap CreateItem(RemoteProp instance) => new RemoteControlImpl(instance);

        private class RemoteControlImpl(GrabbableObject remoteObject) : BaseScrapItem(remoteObject), IRemoteControlScrap
        {
            protected override string ScrapName => "Remote Control";
        }
    }
}