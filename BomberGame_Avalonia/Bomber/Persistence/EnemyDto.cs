namespace Bomber.Persistence;

/// <summary>
/// Data transfer object for the Enemy class.
/// </summary>
public class EnemyDto
{
    /// <summary>
    /// Gets the horizontal coordinate of the enemy.
    /// </summary>
    public int PositionX { get; }

    /// <summary>
    /// Gets the vertical coordinate of the enemy.
    /// </summary>
    public int PositionY { get; }

    /// <summary>
    /// Gets the current direction of the enemy.
    /// </summary>
    public DirectionDto CurrentDirection { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EnemyDto"/> class.
    /// </summary>
    /// <param name="positionY">The vertical coordinate of the enemy.</param>
    /// <param name="positionX">The horizontal coordinate of the enemy.</param>
    /// <param name="currentDirection">The current direction of the enemy.</param>
    public EnemyDto(int positionY, int positionX, DirectionDto currentDirection)
    {
        PositionY = positionY;
        PositionX = positionX;
        CurrentDirection = currentDirection;
    }
}
