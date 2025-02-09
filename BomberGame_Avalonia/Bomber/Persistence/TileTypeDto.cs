namespace Bomber.Persistence;

/// <summary>
/// Data transfer object for the TileType enum.
/// </summary>
public class TileTypeDto
{
    /// <summary>
    /// Gets the value of the tile type.
    /// </summary>
    public int Value { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TileTypeDto"/> class.
    /// </summary>
    /// <param name="value">The enum value</param>
    public TileTypeDto(int value)
    {
        Value = value;
    }
}