
using System;
using ContentLib.API.Model.Terminal;
using ContentLib.Core.Utils;
using InteractiveTerminalAPI.UI.Screen;

namespace ContentLib.Core.Model.Terminal;

public class SettingPage : BoxedScreen
{
    public SettingPage(ConfigKey configKey, Action switchAction, Action updateText)
    {
        Title = ConfigManager.KeyToString(configKey);
        var settingMenu = new SettingMenu(configKey, updateText);
        settingMenu.InitSwitchAction(switchAction);
        elements = [TerminalUIFactory.CreateEmptyTextElement, settingMenu];
    }
    
    
}