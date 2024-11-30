
using System;
using ContentLib.API.Model.Terminal;
using ContentLib.Core.Utils;
using InteractiveTerminalAPI.UI.Screen;

namespace ContentLib.Core.Model.Terminal;

/// <summary>
/// A specialised page for the InteractiveTerminalAPI that is responsible for showing the Setting Menu for a specified
/// Config file value.
/// </summary>
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