using System;

namespace Bomber.Persistence;

/// <summary>
/// Represents a custom exception for data-related errors.
/// </summary>
public class BomberDataException : Exception
{
    public BomberDataException() {}
    public BomberDataException(String message) : base(message) {}
    public BomberDataException(String message, Exception innerException) : base(message, innerException) {}
}