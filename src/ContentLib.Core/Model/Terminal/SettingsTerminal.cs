using System.Collections.Generic;
using ContentLib.API.Model.Terminal;
using InteractiveTerminalAPI.UI.Application;
using InteractiveTerminalAPI.UI.Cursor;
using InteractiveTerminalAPI.UI.Screen;

namespace ContentLib.Core.Model.Terminal;

internal class SettingsTerminal : InteractiveTerminalApplication
{
    public override void Initialization()
    {
        CursorMenu cursorMenu = TerminalUIFactory.CreateCursorMenu(InitMenuCursors());
        IScreen screen = TerminalUIFactory.CreateBoxedScreen("Settings", [
            TerminalUIFactory.CreateTextElement(" "),
            cursorMenu
        ]);
        currentCursorMenu = cursorMenu;
        currentScreen = screen; 
    }

    private CursorElement[] InitMenuCursors()
    {
        var cursorElements = new List<CursorElement>();
        cursorElements.Add(TerminalUIFactory.CreateCursorElement("Logging", null));
        cursorElements.Add(TerminalUIFactory.CreateCursorElement("Events", null));
        cursorElements.Add(TerminalUIFactory.CreateCursorElement("Dependancies", null));
        return cursorElements.ToArray();
    }

  
}