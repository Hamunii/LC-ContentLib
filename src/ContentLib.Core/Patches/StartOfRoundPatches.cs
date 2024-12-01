using ContentLib.API.Model.Event;

namespace ContentLib.Core.Patches;
/// <summary>
/// <p>Patches relevant to the StartOfRound instance, specifically for initialising host / client-side logic.</p>
/// <i>Developer Note: Currently a singular functional method patching StartOfRound.Awake, class exists incase future
/// patches required in core-logic.</i> 
/// </summary>
public class StartOfRoundPatches
{
    public static void Init()
    {
        On.StartOfRound.Awake += (orig, self) =>
        {
            orig(self);
            GameEventManager.Instance.IsHost = self.IsHost;
        };
    }
}