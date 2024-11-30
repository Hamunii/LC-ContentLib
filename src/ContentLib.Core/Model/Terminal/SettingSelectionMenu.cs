using System;
using System.Collections.Generic;
using ContentLib.API.Model.Terminal;
using ContentLib.Core.Utils;
using InteractiveTerminalAPI.UI.Cursor;
using InteractiveTerminalAPI.UI.Screen;

namespace ContentLib.Core.Model.Terminal;
/// <summary>
/// A Cursor Menu for the InteractiveTerminalAPI that displays the various settings available to that subsection of the
/// Config File. E.g: The "Logging" menu, would have all the Config File values for if certain types of logger should be
/// visible.
/// </summary>
public class SettingSelectionMenu : CursorMenu
{
    /// <summary>
    /// Dictionary that directly relates a specific Cursor Element to a specified SettingPage value. This is so, when
    /// selected by the player, the specified cursor element will change the screen to that specified Setting page. 
    /// </summary>
    private readonly Dictionary<CursorElement, SettingPage> _menus = new();
    
    /// <summary>
    /// Action which will switch the setting screen to another one, when called. <i>(Developer Note: This is required
    /// because WhiteSpike has built this thing to only allow refreshing / switching the screen from the
    /// InteractiveTerminalApplication implementation. Dependency Injection go brt!)</i>
    /// </summary>
    private readonly Action _switchToSettingPageAction;

    /// <summary>
    /// Constructor that initialises the menu , via a specified key, action for switching the screen back to the primary
    /// settings screen, and an action for switching screen to a specified SettingPage.
    /// </summary>
    /// <param name="configKeyFilter">The filter to decide which Config File Setting values to be chosen for addition to
    /// the menu.</param>
    /// <param name="switchToSettingPageAction">The action for switching to a specified Setting Page</param>
    /// <param name="switchBackAction">The action for switching back to the Primary Settings Page.</param>
    /// <param name="updateText">Dependency Injected Action for refreshing text on the Terminal.</param>
    public SettingSelectionMenu(string configKeyFilter, Action switchToSettingPageAction, Action switchBackAction,
        Action updateText)
    {
        _switchToSettingPageAction = switchToSettingPageAction;
        elements = InitElements(configKeyFilter, switchBackAction, updateText);
    }

    /// <summary>
    /// The currently selected screen (as decided by the currently selected cursor element).
    /// </summary>
    public BoxedScreen? _selectedScreen { get; private set; }

    /// <summary>
    /// Initialises the elements of the menu, based on the given config file value filter. 
    /// </summary>
    /// <param name="configKeyFilter">The filter to use for deciding which settings to make a menu for
    /// (e.g: All the "logging" settings).</param>
    /// <param name="switchBackAction">The action to switch the screen back to the primary settings terminal screen.</param>
    /// <param name="updateText">Action allowing for the refresh of the terminal screen.</param>
    /// <returns>The completed array of elements for the menu.</returns>
    private CursorElement[] InitElements(string configKeyFilter, Action switchBackAction, Action updateText)
    {
        List<CursorElement> elementsList = new();
        foreach (ConfigKey configKey in Enum.GetValues(typeof(ConfigKey)))
        {
            if (!ConfigManager.KeyToSection(configKey).Contains(configKeyFilter))
                continue;
            SettingPage page = new(configKey, switchBackAction, updateText);
            CursorElement settingElement =
                TerminalUIFactory.CreateCursorElement(ConfigManager.KeyToString(configKey), SwitchPageToSelected);
            _menus.Add(settingElement, page);
            elementsList.Add(settingElement);
        }

        elementsList.Add(TerminalUIFactory.CreateCursorElement("Save Changes", ConfigManager.Instance.SaveConfig));
        elementsList.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.Ordinal));
        return elementsList.ToArray();
    }
    
    /// <summary>
    /// Changes the selected screen via the currently selected cursor index, and then refreshes the screen via the main
    /// Settings Terminal. 
    /// </summary>
    private void SwitchPageToSelected()
    {
        CLLogger.Instance.DebugLog($"Switching to page: {cursorIndex}");
        _selectedScreen = _menus[elements[cursorIndex]];
        CLLogger.Instance.DebugLog($"Switched to page: {_selectedScreen.Title}");
        _switchToSettingPageAction.Invoke();
    }
}