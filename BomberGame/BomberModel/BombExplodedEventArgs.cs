using System;

namespace BomberModel;

/// <summary>
/// Event arguments for when a bomb explodes.
/// </summary>
public class BombExplodedEventArgs : EventArgs
{
    /// <summary>
    /// Gets the position where the bomb exploded.
    /// </summary>
    public Position Position { get; }

    /// <summary>
    /// Gets the radius of the explosion.
    /// </summary>
    public int ExplosionRadius { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BombExplodedEventArgs"/> class.
    /// </summary>
    /// <param name="position">The position of the explosion.</param>
    /// <param name="explosionRadius">The radius of the explosion.</param>
    public BombExplodedEventArgs(Position position, int explosionRadius)
    {
        Position = position;
        ExplosionRadius = explosionRadius;
    }
}