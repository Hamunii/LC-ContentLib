using System;

namespace ContentLib.EnemyAPI.Exceptions;

/// <summary>
/// An exception that's thrown when validation for <see cref="EnemyDefinition"/> fails during registration.
/// </summary>
/// <inheritdoc/>
public class EnemyDefinitionRegistrationException(string message) : Exception(message) { }