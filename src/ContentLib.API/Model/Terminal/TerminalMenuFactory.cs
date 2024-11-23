using System;
using InteractiveTerminalAPI.UI;
using InteractiveTerminalAPI.UI.Cursor;
using InteractiveTerminalAPI.UI.Page;
using InteractiveTerminalAPI.UI.Screen;

namespace ContentLib.API.Model.Terminal;

public class TerminalUIFactory
{
    public static CursorElement CreateCursorElement(string name, Action? action) =>
        new()
        {
            Name = name,
            Action = action
        };

    public static CursorMenu CreateCursorMenu(params CursorElement[] elements) =>
        new()
        {
            cursorIndex = 0,
            elements = elements
        };

    public static IScreen CreateBoxedScreen(string title, ITextElement[] elements) =>
        new BoxedScreen()
        {
            Title = title,
            elements = elements
        };

    public static TextElement CreateTextElement(string text) => new TextElement() { Text = text };
    
    public static PageCursorElement CreatePageCursorElement(CursorMenu[] cursorMenus ) => new PageCursorElement()
    {
        cursorMenus = cursorMenus
    };
}