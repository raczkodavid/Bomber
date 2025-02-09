namespace BomberPersistence;

/// <summary>
/// Data transfer object for the Direction enum.
/// </summary>
public class DirectionDto
{
    /// <summary>
    /// Gets the value of the direction.
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DirectionDto"/> class.
    /// </summary>
    /// <param name="value">The value of the direction.</param>
    public DirectionDto(int value)
    {
        Value = value;
    }
}
