using System;

namespace Bomber.Model;

/// <summary>
/// Event arguments for when a bomb is placed.
/// </summary>
public class BombPlacedEventArgs : EventArgs
{
    /// <summary>
    /// Gets the position where the bomb has been placed.
    /// </summary>
    public Position BombPosition { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BombPlacedEventArgs"/> class.
    /// </summary>
    /// <param name="bombPosition">The position where the bomb has been placed.</param>
    public BombPlacedEventArgs(Position bombPosition)
    {
        BombPosition = bombPosition;
    }
}