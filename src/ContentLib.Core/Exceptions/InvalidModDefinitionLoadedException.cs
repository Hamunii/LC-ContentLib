using System;

namespace ContentLib.Core.Exceptions;

/// <summary>
/// An exception that's thrown when loading a ModDefinition ScriptableObject with invalid values.
/// </summary>
/// <param name="emptyField">The field that was empty.</param>
public class InvalidModDefinitionLoadedException(string emptyField)
    : Exception($"A {nameof(ModDefinition)} must not have empty fields! Field: {emptyField}") { }