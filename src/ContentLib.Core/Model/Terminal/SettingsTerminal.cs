using System.Collections.Generic;
using ContentLib.API.Model.Terminal;
using ContentLib.Core.Model.Managers;
using ContentLib.Core.Utils;
using InteractiveTerminalAPI.UI.Application;
using InteractiveTerminalAPI.UI.Cursor;
using InteractiveTerminalAPI.UI.Screen;

namespace ContentLib.Core.Model.Terminal;
/// <summary>
/// The Interactive Terminal Application representing the custom terminal screen, generated via the
/// InteractiveTerminalAPI, for the purposes of navigating to, seeing, and changing Config File Settings in-game.
/// </summary>
internal class SettingsTerminal : InteractiveTerminalApplication
{
    private readonly SettingsManager _settingsManager = SettingsManager.Instance;

   
    #region Menus
    /// <summary>
    /// The Cursor Menu for the main terminal screen, showing the sub-categories of the Config File Settings.
    /// </summary>
    private CursorMenu _mainMenu;
    
    /// <summary>
    /// The Setting Selection Menu that corresponds to the currently selected Setting Page. 
    /// </summary>
    private SettingSelectionMenu _settingsMenu;
    #endregion
    
    #region Screens
    /// <summary>
    /// The main screen of the settings terminal. Shows all the sub-categories of the Config File Settings.
    /// </summary>
    private IScreen _mainScreen;
    
    /// <summary>
    /// The Screen showing the Config File Settings relevant to the displaying of Bepinex console logs.
    /// </summary>
    private IScreen _loggingScreen;
    #endregion
    
    /// <summary>
    /// Initialization method of the InteractiveTerminalAPI, that initialises the Settings Terminal on-call. 
    /// </summary>
    public override void Initialization()
    {
        _mainMenu = TerminalUIFactory.CreateCursorMenu(InitMenuCursors());
        
        _mainScreen = TerminalUIFactory.CreateBoxedScreen("Settings"
            , [
                TerminalUIFactory.CreateTextElement(" ")
                , _mainMenu
            ]);
        
        _settingsMenu = new SettingSelectionMenu("Logging",SettingPageSwitch, LoggingPageSwitch, UpdateText);
        _loggingScreen = TerminalUIFactory.CreateBoxedScreen("Logging Settings"
            , [TerminalUIFactory.CreateTextElement(" "), _settingsMenu]);
        
        currentCursorMenu = _mainMenu;
        currentScreen = _mainScreen;
    }

    /// <summary>
    /// Initialises the Cursor Elements of the Main Terminal screen menu. Showing the sub-categories of Config File
    /// Settings.
    /// </summary>
    /// <returns>The initialised Terminal Cursor Elements.</returns>
    private CursorElement[] InitMenuCursors()
    {
        var cursorElements = new List<CursorElement>
        {
            TerminalUIFactory
                .CreateCursorElement("Logging", LoggingPageSwitch),
            TerminalUIFactory
                .CreateCursorElement("Events", _settingsManager.MoveToEventsSettingPage),
            TerminalUIFactory
                .CreateCursorElement("Dependencies", _settingsManager.MoveDependenciesToSettingPage),
        };
        return cursorElements.ToArray();
    }
    
    /// <summary>
    /// Action which switches the Terminal's screen to the Logging Setting Page.
    /// </summary>
    private void LoggingPageSwitch()
    {
        SwitchScreen(_loggingScreen,_settingsMenu,false);
    }

    /// <summary>
    /// Set the Page to the currently selected screen within the Settings Menu. 
    /// </summary>
    private void SettingPageSwitch()
    {
        CLLogger.Instance.DebugLog("Invoking SettingPageSwitch");
        BoxedScreen settingsScreen = _settingsMenu?._selectedScreen;
        CLLogger.Instance.DebugLog($"Setting logging screen to {settingsScreen.Title}");

        if (settingsScreen == null)
            return;
        SwitchScreen(settingsScreen,(CursorMenu) settingsScreen?.elements[1],false );
    }

  
    
  
}