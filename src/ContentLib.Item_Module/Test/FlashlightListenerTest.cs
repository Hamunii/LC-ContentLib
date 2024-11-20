using ContentLib.API.Model.Event;
using ContentLib.API.Model.Item.Tools.Types;
using ContentLib.Core.Model.Event.Listener;
using ContentLib.Core.Utils;

namespace ContentLib.Item_Module.Test;

public class FlashlightListenerTest : IListener
{ 
    [EventDelegate]
    private void OnItemActivate(ItemActivationEvent itemActivationEvent)
    {
        CLLogger.Instance.Log($"OnItemActivate: {itemActivationEvent}");
        if (itemActivationEvent.Item is IFlashlight flashlight)
        {
            flashlight.Weight = 10;
        }
    }
}