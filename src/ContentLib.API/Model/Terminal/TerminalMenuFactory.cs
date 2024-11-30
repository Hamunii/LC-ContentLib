using System;
using InteractiveTerminalAPI.UI;
using InteractiveTerminalAPI.UI.Cursor;
using InteractiveTerminalAPI.UI.Page;
using InteractiveTerminalAPI.UI.Screen;

namespace ContentLib.API.Model.Terminal;
/// <summary>
/// Factory for creating the primary components of an Interactive Terminal UI. Used to save time in the creation of
/// custom UI Screens for the API.
/// </summary>
public class TerminalUIFactory
{
    /// <summary>
    /// Creates an empty text field, usually used to have a gap after the title of a screen.
    /// </summary>
    /// <returns>The Constructed Empty Text Element.</returns>
    public static TextElement CreateEmptyTextElement => new TextElement() { Text = " " };
    
    /// <summary>
    /// Creates a "Cursor Element" , an element that can be selected and, upon pressing enter, perform a specified
    /// action.
    /// </summary>
    /// <param name="name">The text name of the element, will be what the user sees as the element's text.</param>
    /// <param name="action">The Action that gets performed when the user presses the button <i>(Developer Note:
    /// Does not accept any params).</i></param>
    /// <returns>The Constructed Cursor Element.</returns>
    public static CursorElement CreateCursorElement(string name, Action? action) =>
        new()
        {
            Name = name,
            Action = action
        };

    /// <summary>
    /// Creates a "Cursor Menu", a collection of Cursor elements to be placed within a Screen.
    /// </summary>
    /// <param name="elements">Array of Cursor Elements to act as the menu's contents.</param>
    /// <returns>The constructed Cursor Menu.</returns>
    public static CursorMenu CreateCursorMenu(params CursorElement[] elements) =>
        new()
        {
            cursorIndex = 0,
            elements = elements
        };

    /// <summary>
    /// Creates a Boxed Screen Component, acts as the full visual view shown upon the Terminal. E.g: A "Settings Screen".
    /// </summary>
    /// <param name="title">The title of the screen.</param>
    /// <param name="elements">The text elements of the screen, this can include both Non-Cursor and Cursor elements.
    /// </param>
    /// <returns>The constructed Boxed Screen.</returns>
    public static IScreen CreateBoxedScreen(string title, ITextElement[] elements) =>
        new BoxedScreen()
        {
            Title = title,
            elements = elements
        };

    /// <summary>
    /// Creates a Text Element, an unselectable Element that can act as a note upon the screen. 
    /// </summary>
    /// <param name="text">The text within the element, this would be shown to the end user.</param>
    /// <returns>The constructetd Text Element.</returns>
    public static TextElement CreateTextElement(string text) => new TextElement() { Text = text };
    
  
    //TODO Experiment with this later... much... much... later...
    public static PageCursorElement CreatePageCursorElement(CursorMenu[] cursorMenus ) => new PageCursorElement()
    {
        cursorMenus = cursorMenus
    };
}