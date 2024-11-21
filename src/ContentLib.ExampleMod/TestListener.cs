using ContentLib.API.Model.Event;
using ContentLib.Core.Model.Event.Listener;

namespace ContentLib.ExampleMod;

public class TestListener: IListener
{
    [EventDelegate]
    private void OnPlayerJumpEvent(PlayerJumpEvent playerJumpEvent)
    {
        playerJumpEvent.Player.TeleportToShip();
    }
}