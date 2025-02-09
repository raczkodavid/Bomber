namespace Bomber.Persistence;

/// <summary>
/// Interface for data access objects.
/// </summary>
public interface IBomberDataAccess
{
    /// <summary>
    /// Loads a game board from the specified path.
    /// </summary>
    /// <param name="path">Path of the file containing the map.</param>
    /// <returns>Data transfer object for GameBoard</returns>
    GameBoardDto LoadGameBoard(string path);
}