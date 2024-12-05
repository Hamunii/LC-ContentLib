using BepInEx;
using ContentLib.API;

namespace ContentLib.ExampleMod;
[BepInPlugin(LCMPluginInfo.PLUGIN_GUID, LCMPluginInfo.PLUGIN_NAME, LCMPluginInfo.PLUGIN_VERSION)]
public class Plugin : ContentLibPlugin
{
    protected override void OnAwake()
    {
        ContentLibAPI.Instance.RegisterListener(new TestListener());
    }
}