using System;
using ContentLib.API.Model.Terminal;
using ContentLib.Core.Utils;
using InteractiveTerminalAPI.UI.Cursor;

namespace ContentLib.Core.Model.Terminal;


public class SettingMenu : CursorMenu
{
    public void InitSwitchAction(Action switchAction) => _switchAction = switchAction;
    private Action? _switchAction;
    private Action? _updateTextAction;
    private readonly ConfigKey _settingType;
    private string _menuText;
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

    private string InitMenuText(string menuText)
    {
        return $"{menuText}: {_configManager.GetConfigValue<bool>(_settingType)}";
    }
    private void ChangeSetting()
    {
        var newValue = !_configManager.GetConfigValue<bool>(_settingType);
        CLLogger.Instance.DebugLog($"Setting {_settingType} to {newValue}");
        _configManager.SetConfigValue(_settingType, newValue);
        CursorElement settingElement = elements[0];
        settingElement.Name = InitMenuText(_menuText);
        _updateTextAction?.Invoke();
    }

    private void GoToPreviousPage() => _switchAction?.Invoke();
}