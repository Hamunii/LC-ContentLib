using System;

namespace ContentLib.Core.Exceptions;

/// <summary>
/// An exception that's thrown when registering a content that's already been registered.
/// </summary>
/// <inheritdoc/>
public class ContentAlreadyRegisteredException(string message) : Exception(message) { }