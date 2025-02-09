using System;
using System.Collections.Generic;

namespace Bomber.Model;

/// <summary>
/// Event arguments for when a bomb explodes.
/// </summary>
public class EnemiesMovedEventArgs : EventArgs
{
    /// <summary>
    /// Gets previous positions of the enemies before movement.
    /// </summary>
    public List<Position> PrevPositions { get; }

    /// <summary>
    /// Gets new positions of the enemies after movement.
    /// </summary>
    public List<Position> NewPositions { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EnemiesMovedEventArgs"/> class.
    /// </summary>
    /// <param name="prevPositions">List of positions before enemies moved.</param>
    /// <param name="newPositions">List of positions enemies are currently on.</param>
    public EnemiesMovedEventArgs(List<Position> prevPositions, List<Position> newPositions)
    {
        PrevPositions = prevPositions;
        NewPositions = newPositions;
    }
}
