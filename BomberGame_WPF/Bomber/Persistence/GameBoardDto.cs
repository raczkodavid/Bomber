using System.Collections.Generic;

namespace Bomber.Persistence;

/// <summary>
/// Data transfer object for the GameBoard class.
/// </summary>
public class GameBoardDto
{
    /// <summary>
    /// 2d array representing the layout of the game board.
    /// </summary>
    public TileTypeDto[,] MapLayout { get; }

    /// <summary>
    /// List of enemies on the game board.
    /// </summary>
    public List<EnemyDto> Enemies { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GameBoardDto"/> class.
    /// </summary>
    /// <param name="mapLayout">The 2D array representing the layout of the game board.</param>
    /// <param name="enemies">The list of enemies on the game board.</param>
    public GameBoardDto(TileTypeDto[,] mapLayout, List<EnemyDto> enemies)
    {
        MapLayout = mapLayout;
        Enemies = enemies;
    }
}
