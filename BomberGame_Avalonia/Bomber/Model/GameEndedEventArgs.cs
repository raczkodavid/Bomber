using System;

namespace Bomber.Model;

/// <summary>
/// Provides data for the GameEnded event.
/// </summary>
public class GameEndedEventArgs : EventArgs
{
    /// <summary>
    /// Gets a value indicating whether the game ended in victory.
    /// </summary>
    public bool IsVictory { get; }

    /// <summary>
    /// Creates a new instance of the <see cref="GameEndedEventArgs"/> class.
    /// </summary>
    /// <param name="isVictory">Indicates whether the player won.</param>
    public GameEndedEventArgs(bool isVictory)
    {
        IsVictory = isVictory;
    }
}