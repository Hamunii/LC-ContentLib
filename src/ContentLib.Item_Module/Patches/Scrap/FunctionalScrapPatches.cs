using System;
using ContentLib.API.Exceptions.Core.Manager;
using ContentLib.API.Model.Event;
using ContentLib.API.Model.Item;
using ContentLib.Core.Model.Managers;
using ContentLib.Core.Utils;
using UnityEngine;

namespace ContentLib.Item_Module.Patches.Scrap
{
    public abstract class BaseFunctionalScrapPatch<T, U> where T : GrabbableObject where U : class
    {
        protected static void Init<V>() where V : BaseFunctionalScrapPatch<T, U>, new()
        {
            Patch<V>();
        }

        private static void Patch<V>() where V : BaseFunctionalScrapPatch<T, U>, new()
        {
            CLLogger.Instance.DebugLog($"Performing {typeof(V).Name}", DebugLevel.CoreEvent);
            On.GrabbableObject.Start += (orig, self) =>
            {
                orig(self);

                if (self is not T targetInstance)
                {
                    CLLogger.Instance.DebugLog($"{typeof(T).Name} check was null.", DebugLevel.CoreEvent);
                    return;
                }

                var patchInstance = new V();

                U itemInstance = patchInstance.CreateItem(targetInstance);
                patchInstance.RegisterItem(itemInstance);
            };
        }

        protected abstract U CreateItem(T instance);

        protected virtual void RegisterItem(U itemInstance)
        {
            if (!(itemInstance is IGameItem item))
                return;
            try
            {
                CLLogger.Instance.DebugLog($"Registering item {itemInstance.GetType().Name}.", DebugLevel.CoreEvent);
                ItemManager.Instance.RegisterOrReplaceItem(item);
                ItemSpawnedEvent spawnedEvent = new ScrapSpawnEvent(item);
                GameEventManager.Instance.Trigger(spawnedEvent);
            }
            catch (Exception ex)
            {
                throw new InvalidItemRegistrationException(item, ex);
            }
            
        }
        private class ScrapSpawnEvent(IGameItem item) : ItemSpawnedEvent
        {
            public override Vector3 Position => item.Location;
            public override IGameItem? Item => item;
        }
    }

 
}