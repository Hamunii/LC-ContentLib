using ContentLib.API.Model.Item;
using ContentLib.Core.Utils;
using ContentLib.Item_Module.Model;

namespace ContentLib.Item_Module.Patches.Scrap
{
    public abstract class BasePatch<T, U> where T : GrabbableObject where U : class
    {
        protected static void Init<V>() where V : BasePatch<T, U>, new()
        {
            Patch<V>();
        }

        private static void Patch<V>() where V : BasePatch<T, U>, new()
        {
            CLLogger.Instance.Log($"Performing {typeof(V).Name}");
            On.GrabbableObject.Start += (orig, self) =>
            {
                orig(self);

                if (self is not T targetInstance)
                {
                    CLLogger.Instance.DebugLog($"{typeof(T).Name} check was null.");
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
            CLLogger.Instance.Log($"Registering item {itemInstance.GetType().Name}.");
            if (itemInstance is IGameItem item)
            {
                ItemManager.Instance.RegisterItem(item);
            }
            else
            {
                CLLogger.Instance.DebugLog("Item does not implement the required interface.");
            }
        }
    }
}