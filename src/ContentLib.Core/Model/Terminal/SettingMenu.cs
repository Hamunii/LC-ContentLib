using System;
using ContentLib.API.Model.Terminal;
using ContentLib.Core.Utils;
using InteractiveTerminalAPI.UI.Cursor;

namespace ContentLib.Core.Model.Terminal;

/// <summary>
/// A Cursor Menu for the InteractiveTerminalAPI that displays a single setting, its current status (true or false) and
/// gives both an option to return to the main Setting Page, or to switch the status of the config setting. 
/// </summary>
public class SettingMenu : CursorMenu
{
    /// <summary>
    /// Initialises the action responsible for switching back to the primary settings screen.
    /// </summary>
    /// <param name="switchAction">The switch action to intialise within the menu.</param>
    public void InitSwitchAction(Action switchAction) => _switchAction = switchAction;
    
    private Action? _switchAction;
    /// <summary>
    /// Action for updating the text on this menu screen (i.e. when false switches to true or vice versa). 
    /// </summary>
    private Action? _updateTextAction;
    
    /// <summary>
    /// The Config file setting this menu is handling.
    /// </summary>
    private readonly ConfigKey _settingType;
    
    /// <summary>
    /// The text to show in this menu (saved so the dynamic switching of true and false is more easily handled). 
    /// </summary>
    private string _menuText;
    
    /// <summary>
    /// Instance of the config manager, for ease of interaction with the config file settings upon player input.
    /// </summary>
    private ConfigManager _configManager = ConfigManager.Instance;
    
    public SettingMenu(ConfigKey settingType, Action updateText)
    {
        _updateTextAction = updateText;
        _menuText = ConfigManager.KeyToString(settingType);
        _settingType = settingType;
        elements = [
            TerminalUIFactory.CreateCursorElement(InitMenuText(_menuText),ChangeSetting)
            ,TerminalUIFactory.CreateCursorElement("Back",GoToPreviousPage)];
    }

    /// <summary>
    /// Initialises the shown text within the menu. Essentially whether to show the player "true" or "false" as the
    /// current status of the setting.
    /// </summary>
    /// <param name="menuText">The menu text to show.</param>
    /// <returns>The text to show within the menu.</returns>
    private string InitMenuText(string menuText)
    {
        return $"{menuText}: {_configManager.GetConfigValue<bool>(_settingType)}";
    }
    
    /// <summary>
    /// Changes the setting from true to false, or vice versa, for the Config file setting this menu is handling.
    /// </summary>
    private void ChangeSetting()
    {
        var newValue = !_configManager.GetConfigValue<bool>(_settingType);
        CLLogger.Instance.DebugLog($"Setting {_settingType} to {newValue}");
        _configManager.SetConfigValue(_settingType, newValue);
        CursorElement settingElement = elements[0];
        settingElement.Name = InitMenuText(_menuText);
        _updateTextAction?.Invoke();
    }

    /// <summary>
    /// Action that will invoke the switch action, and return the terminal UI to the primary setting screen.
    /// </summary>
    private void GoToPreviousPage() => _switchAction?.Invoke();
}