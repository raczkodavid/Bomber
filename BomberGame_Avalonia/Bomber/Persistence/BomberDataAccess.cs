using System;
using System.Collections.Generic;
using System.IO;

namespace Bomber.Persistence
{
    /// <summary>
    /// Provides methods to load game board data from a file or stream.
    /// </summary>
    public class BomberDataAccess : IBomberDataAccess
    {
        /// <summary>
        /// Loads the game board from the specified stream containing the game board data.
        /// </summary>
        /// <param name="stream">The stream containing the game board data.</param>
        /// <returns>A <see cref="GameBoardDto"/> object representing the loaded game board.</returns>
        /// <exception cref="BomberDataException">Thrown when there is an error loading the game board.</exception>
        public GameBoardDto LoadGameBoard(Stream stream)
        {
            try
            {
                using StreamReader sr = new StreamReader(stream);

                // Read map size and enemy count (Num Num)
                string[] mapInfo = sr.ReadLine()?.Split(" ") ?? [];
                if (mapInfo.Length != 2)
                    throw new BomberDataException("Invalid map size and enemy count");

                int mapSize = int.Parse(mapInfo[0]);
                if (mapSize < 10) // minimum map size
                    throw new BomberDataException("Map size must be at least 10");

                int enemyCount = int.Parse(mapInfo[1]);
                if (enemyCount < 1) // minimum enemy count
                    throw new BomberDataException("There must be at least 1 enemy");

                // Read the map layout
                TileTypeDto[,] mapLayout = new TileTypeDto[mapSize, mapSize];
                for (int i = 0; i < mapSize; i++)
                {
                    string line = sr.ReadLine() ?? string.Empty;
                    string[] splitLine = line.Split(" ");

                    if (splitLine.Length != mapSize)
                        throw new BomberDataException($"Invalid map layout.");

                    for (int j = 0; j < mapSize; j++)
                    {
                        string currentPiece = splitLine[j];
                        mapLayout[i, j] = new TileTypeDto(currentPiece == "#" ? 1 : 0);
                    }
                }

                // Read the enemies
                List<EnemyDto> enemies = new List<EnemyDto>();
                for (int i = 0; i < enemyCount; i++)
                {
                    string[] enemyInfo = sr.ReadLine()?.Split(" ") ?? [];
                    if (enemyInfo.Length != 3)
                        throw new BomberDataException("Invalid enemy info");

                    int positionY = int.Parse(enemyInfo[0]);
                    int positionX = int.Parse(enemyInfo[1]);

                    if (positionX < 0 || positionX >= mapSize || positionY < 0 || positionY >= mapSize)
                        throw new BomberDataException("Enemy position is out of bounds");

                    DirectionDto currentDirection = new DirectionDto(int.Parse(enemyInfo[2]));
                    enemies.Add(new EnemyDto(positionY, positionX, currentDirection));
                }

                return new GameBoardDto(mapLayout, enemies);
            }
            catch (Exception ex)
            {
                throw new BomberDataException("Error loading game board", ex);
            }
        }

        /// <summary>
        /// Loads the game board from the specified file path.
        /// </summary>
        /// <param name="path">The path to the file containing the game board data.</param>
        /// <returns>A <see cref="GameBoardDto"/> object representing the loaded game board.</returns>
        /// <exception cref="BomberDataException">Thrown when there is an error loading the game board.</exception>
        public GameBoardDto LoadGameBoard(string path)
        {
            try
            {
                using FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                return LoadGameBoard(fs);
            }
            catch (Exception ex)
            {
                throw new BomberDataException("Error loading game board", ex);
            }
        }
    }
}
