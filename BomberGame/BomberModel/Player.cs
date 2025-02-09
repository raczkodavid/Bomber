namespace BomberModel;

/// <summary>
/// Represents the player in the Bomber game.
/// </summary>
public class Player
{
    #region Public Properties

    /// <summary>
    /// Gets the position of the player.
    /// </summary>
    public Position Position { get; private set; }
    
    public bool IsAlive { get; set; } = true;

    #endregion

    #region Public Methods

    /// <summary>
    /// Places a bomb at the player's current position.
    /// </summary>
    /// <returns>A new Bomb object placed at the player's position.</returns>
    public Bomb PlaceBomb() => new Bomb(Position);

    /// <summary>
    /// Moves the player in the specified direction.
    /// </summary>
    /// <param name="dir">The direction to move the player.</param>
    public void Move(Direction dir) => Position += Position.GetMovementVector(dir);

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the Player class with the specified position.
    /// </summary>
    /// <param name="position">The initial position of the player.</param>
    public Player(Position position) => Position = position;

    /// <summary>
    /// Initializes a new instance of the Player class with the default position (0, 0).
    /// </summary>
    public Player() => Position = new Position(0, 0);
    
    #endregion
}