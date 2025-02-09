using System;

namespace Bomber.Model;

/// <summary>
/// Represents a position with Y and X coordinates.
/// </summary>
public class Position
{
    #region Properties
    
    /// <summary>
    /// Gets the Y coordinate of the position. (vertical)
    /// </summary>
    public int Y { get; }
    
    /// <summary>
    /// Gets the X coordinate of the position. (horizontal)
    /// </summary>
    public int X { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Position"/> class with the specified coordinates.
    /// </summary>
    /// <param name="y">The Y coordinate.</param>
    /// <param name="x">The X coordinate.</param>
    public Position(int y, int x)
    {
        Y = y;
        X = x;
    }

    #endregion

    #region Public Static Methods

    /// <summary>
    /// Gets the movement "vector" for the specified direction.
    /// </summary>
    /// <param name="dir">The direction of movement.</param>
    /// <returns>A new <see cref="Position"/> representing the movement vector.</returns>
    public static Position GetMovementVector(Direction dir)
    {
        switch (dir)
        {
            case Direction.UP:
                return new Position(-1, 0);
            case Direction.DOWN:
                return new Position(1, 0);
            case Direction.LEFT:
                return new Position(0, -1);
            case Direction.RIGHT:
                return new Position(0, 1);
            default:
                return new Position(0, 0);
        }
    }

    #endregion

    #region Public Operator Overloads

    /// <summary>
    /// Determines whether two <see cref="Position"/> instances are equal.
    /// </summary>
    /// <param name="p1">The first position.</param>
    /// <param name="p2">The second position.</param>
    /// <returns><c>true</c> if the positions are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(Position p1, Position p2) => p1.Y == p2.Y && p1.X == p2.X;

    /// <summary>
    /// Determines whether two <see cref="Position"/> instances are not equal.
    /// </summary>
    /// <param name="p1">The first position.</param>
    /// <param name="p2">The second position.</param>
    /// <returns><c>true</c> if the positions are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(Position p1, Position p2) => !(p1 == p2);

    /// <summary>
    /// Adds two <see cref="Position"/> instances.
    /// </summary>
    /// <param name="p1">The first position.</param>
    /// <param name="p2">The second position.</param>
    /// <returns>A new <see cref="Position"/> that is the sum of the two positions.</returns>
    public static Position operator +(Position p1, Position p2) => new Position(p1.Y + p2.Y, p1.X + p2.X);

    /// <summary>
    /// Subtracts one <see cref="Position"/> from another.
    /// </summary>
    /// <param name="p1">The first position.</param>
    /// <param name="p2">The second position.</param>
    /// <returns>A new <see cref="Position"/> that is the difference of the two positions.</returns>
    public static Position operator -(Position p1, Position p2) => new Position(p1.Y - p2.Y, p1.X - p2.X);

    #endregion

    #region Public Methods

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="Position"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current position.</param>
    /// <returns><c>true</c> if the specified object is equal to the current position; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Position)obj);
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current position.</returns>
    public override int GetHashCode() => HashCode.Combine(Y, X);
    
    #endregion

    #region Private Methods

    /// <summary>
    /// Determines whether the specified <see cref="Position"/> is equal to the current <see cref="Position"/>.
    /// </summary>
    /// <param name="other">The position to compare with the current position.</param>
    /// <returns><c>true</c> if the specified position is equal to the current position; otherwise, <c>false</c>.</returns>
    private bool Equals(Position other) =>  Y == other.Y && X == other.X;

    #endregion
}