using ContentLib.API.Model.Item.Tools.Types;
using ContentLib.Core.Model.Event.Listener;
using ContentLib.Core.Utils;
using ContentLib.Item_Module.Events;
 
namespace ContentLib.Item_Module.Test;

public class FlashlightListenerTest : IListener
{ 
    [EventDelegate]
    private void OnItemActivate(OnItemActivationEvent itemActivationEvent)
    {
        CLLogger.Instance.Log($"OnItemActivate: {itemActivationEvent}");
        if (itemActivationEvent.Item is IFlashlight flashlight)
        {
            //CLLogger.Instance.Log($"A flashlight was turned on with ID: {flashlight.Id}");
            Landmine.SpawnExplosion(flashlight.Location,true,default,default,default,1F);
        }
    }
}