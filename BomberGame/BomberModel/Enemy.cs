using System;

namespace BomberModel;

public class Enemy
{
    #region Public Properties

    /// <summary>
    /// Gets the current position of the enemy.
    /// </summary>
    public Position Position { get; private set; }

    #endregion

    #region Private Fields
    
    /// <summary>
    /// Random number generator for changing directions, when colliding with walls.
    /// </summary>
    private readonly Random _random;
    
    /// <summary>
    /// Current direction the enemy is facing.
    /// </summary>
    private Direction _currentDirection;

    #endregion

    #region Constructors
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Enemy"/> class.
    /// </summary>
    /// <param name="position">Position of the enemy</param>
    /// <param name="startDirection">Starting direction the enemy is facing</param>
    public Enemy(Position position, Direction startDirection)
    {
        Position = position;
        _random = new Random();
        _currentDirection = startDirection;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Changes the direction where the enemy is facing.
    /// </summary>
    public void ChangeDirection()
    {
        Direction newDirection;
        do
        { 
            newDirection = (Direction)_random.Next(4);
        } 
        while (newDirection == _currentDirection);

        _currentDirection = newDirection;
    }
    
    /// <summary>
    /// Calculates where the enemy would move next.
    /// </summary>
    /// <returns>Next position the enemy would move to.</returns>
    public Position GetNextStepPosition() => Position + Position.GetMovementVector(_currentDirection);
    
    /// <summary>
    /// Moves the enemy to the next position, based on the current direction.
    /// </summary>
    public void Move() => Position = GetNextStepPosition();
    
    #endregion
}