using ContentLib.API.Model.Item.Tools.Types;
using ContentLib.Core.Model.Event.Listener;
using ContentLib.Core.Utils;
using ContentLib.Item_Module.Events;
 
namespace ContentLib.Item_Module.Test;

public class FlashlightListenerTest : IListener
{ 
    [EventDelegate]
    private void OnItemActivate(ItemActivationEvent itemActivationEvent)
    {
        CLLogger.Instance.Log($"OnItemActivate: {itemActivationEvent}");
        if (itemActivationEvent.Item is IFlashlight flashlight)
        {
            CLLogger.Instance.Log($"OnItemActivate: {itemActivationEvent}");
        }
    }
}