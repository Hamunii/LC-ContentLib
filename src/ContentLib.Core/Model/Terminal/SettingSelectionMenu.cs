using System;
using System.Collections.Generic;
using ContentLib.API.Model.Terminal;
using ContentLib.Core.Utils;
using InteractiveTerminalAPI.UI.Cursor;
using InteractiveTerminalAPI.UI.Screen;

namespace ContentLib.Core.Model.Terminal;

public class SettingSelectionMenu : CursorMenu
{
    private Dictionary<CursorElement, SettingPage> _menus = new();
    public BoxedScreen? _selectedScreen { get; private set; }
    private Action _switchToSettingScreenAction;
    public SettingSelectionMenu(string configKeyFilter, Action switchToSettingScreenAction, Action switchBackAction,
        Action updateText)
    {
        _switchToSettingScreenAction = switchToSettingScreenAction;
        elements = InitElements(configKeyFilter, switchBackAction, updateText);
    }

    private CursorElement[] InitElements(string configKeyFilter, Action switchBackAction, Action updateText)
    {
        List<CursorElement> elementsList = new();
        foreach (ConfigKey configKey in Enum.GetValues(typeof(ConfigKey)))
        {
            if (!ConfigManager.KeyToSection(configKey).Contains(configKeyFilter))
                continue;
            SettingPage page = new (configKey, switchBackAction, updateText);
            CursorElement settingElement =
                TerminalUIFactory.CreateCursorElement(ConfigManager.KeyToString(configKey), SwitchPageToSelected);
            _menus.Add(settingElement, page);
            elementsList.Add(settingElement);
        }
        elementsList.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.Ordinal));
        return elementsList.ToArray();
    }

    private void SwitchPageToSelected()
    {
        CLLogger.Instance.DebugLog($"Switching to page: {cursorIndex}");
        _selectedScreen = _menus[elements[cursorIndex]];
        CLLogger.Instance.DebugLog($"Switched to page: {_selectedScreen.Title}");
        _switchToSettingScreenAction.Invoke();
    }
    
}