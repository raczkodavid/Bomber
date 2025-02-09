using System;

namespace BomberModel;

/// <summary>
/// Provides data for the PlayerMoved event.
/// </summary>
public class PlayerMovedEventArgs : EventArgs
{
    /// <summary>
    /// Gets the previous position of the player.
    /// </summary>
    public Position PreviousPosition { get; }

    /// <summary>
    /// Gets the new position of the player.
    /// </summary>
    public Position NewPosition { get; }

    /// <summary>
    /// Creates a new instance of the <see cref="PlayerMovedEventArgs"/> class.
    /// </summary>
    /// <param name="previousPosition">The previous position of the player.</param>
    /// <param name="newPosition">The new position the player is currently on.</param>
    public PlayerMovedEventArgs(Position previousPosition, Position newPosition)
    {
        PreviousPosition = previousPosition;
        NewPosition = newPosition;
    }
}