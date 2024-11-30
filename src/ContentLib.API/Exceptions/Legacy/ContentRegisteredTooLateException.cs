using System;

namespace ContentLib.Core.Exceptions;

/// <summary>
/// An exception that's thrown when registering content too late.
/// </summary>
/// <inheritdoc/>
public class ContentRegisteredTooLateException(string message) : Exception(message) { }