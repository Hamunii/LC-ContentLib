using System.Collections.Generic;
using ContentLib.API.Model.Terminal;
using ContentLib.Core.Model.Managers;
using ContentLib.Core.Utils;
using InteractiveTerminalAPI.UI.Application;
using InteractiveTerminalAPI.UI.Cursor;
using InteractiveTerminalAPI.UI.Screen;

namespace ContentLib.Core.Model.Terminal;

internal class SettingsTerminal : InteractiveTerminalApplication
{
    private readonly SettingsManager _settingsManager = SettingsManager.Instance;
    private IScreen _mainScreen;
    private CursorMenu _mainMenu;
    private SettingSelectionMenu _settingsMenu;
    private IScreen _loggingScreen;
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

    private CursorElement[] InitLoggingSettingsMenuCursors() => new List<CursorElement> { 
        TerminalUIFactory.CreateCursorElement("Logs Enabled?", null),
        TerminalUIFactory.CreateCursorElement("Show Unity Logs?", null)
    }.ToArray();

    private void LoggingPageSwitch()
    {
        SwitchScreen(_loggingScreen,_settingsMenu,false);
    }

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