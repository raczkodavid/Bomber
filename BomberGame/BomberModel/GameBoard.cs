using System;
using System.Collections.Generic;
using System.Linq;

namespace BomberModel;

public class GameBoard
{
    #region Private Fields

    /// <summary>
    /// Map layout of the game board, only containing where walls are.
    /// </summary>
    private readonly TileType[,] _mapLayout;

    /// <summary>
    /// Stores the enemies on the game board.
    /// </summary>
    private readonly List<Enemy> _enemies;

    /// <summary>
    /// Stores where bombs are placed on the game board.
    /// </summary>
    private readonly List<Position> _bombPositions;

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the size of the game board.
    /// </summary>
    public int Size { get; } 

    /// <summary>
    /// Gets the number of remaining enemies on the game board.
    /// </summary>
    public int RemainingEnemies => _enemies.Count;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="GameBoard"/> class.
    /// </summary>
    public GameBoard(TileType[,] mapLayout, List<Enemy> enemies)
    {
        _mapLayout = mapLayout;
        Size = mapLayout.GetLength(0);
        _enemies = enemies;
        _bombPositions = new List<Position>();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Checks if a wall tile is at the given position.
    /// </summary>
    public bool IsWallTile(Position pos) => _mapLayout[pos.Y, pos.X] == TileType.WALL;

    /// <summary>
    /// Checks if an enemy is at the given position.
    /// </summary>
    public bool IsEnemyTile(Position pos) => _enemies.Any(e => e.Position == pos);

    /// <summary>
    /// Checks if a bomb is at the given position.
    /// </summary>
    public bool IsBombTile(Position pos) => _bombPositions.Contains(pos);

    /// <summary>
    /// Checks if a position is within the game board and not a wall tile.
    /// </summary>
    public bool CanMoveTo(Position pos)
    {
        return pos.Y >= 0 && pos.Y < Size && pos.X >= 0 && pos.X < Size && !IsWallTile(pos);
    }

    /// <summary>
    /// Kills all enemies in a given radius of an explosion.
    /// </summary>
    /// <param name="explosionCenter">Position where the bomb has exploded.</param>
    /// <param name="explosionRadius">Radius of the explosion.</param>
    /// <returns>Number of killed enemies.</returns>
    public int KillEnemiesInRadius(Position explosionCenter, int explosionRadius)
    {
        int killedEnemies = 0;

        // Temporary list to store enemies to be removed
        List<Enemy> enemiesToRemove = new List<Enemy>();

        foreach (Enemy enemy in _enemies)
        {
            if (IsEntityInRadius(enemy.Position, explosionCenter, explosionRadius))
            {
                enemiesToRemove.Add(enemy);
                killedEnemies++;
            }
        }

        // Now remove all enemies that are marked for removal
        foreach (Enemy enemy in enemiesToRemove)
            _enemies.Remove(enemy);

        return killedEnemies;
    }


    /// <summary>
    /// Moves all enemies on the game board.
    /// </summary>
    /// <returns>A tuple containing the previous and new positions of alive enemies</returns>
    public (List<Position> prevPositions, List<Position> newPositions) MoveEnemies()
    {
        List<Position> prevPositions = new List<Position>();
        List<Position> newPositions  = new List<Position>();

        foreach (Enemy enemy in _enemies)
        {
            // Change direction until a valid move is found
            while (!CanMoveTo(enemy.GetNextStepPosition())) 
                enemy.ChangeDirection();
            
            prevPositions.Add(enemy.Position);
            enemy.Move();
            newPositions.Add(enemy.Position);
        }

        return (prevPositions, newPositions);
    }
    /// <summary>
    /// Checks if an entity is within a given radius of a center position.
    /// </summary>
    public bool IsEntityInRadius(Position entityPos, Position centerPos, int radius)
    {
        return Math.Abs(entityPos.Y - centerPos.Y) <= radius &&
               Math.Abs(entityPos.X - centerPos.X) <= radius;
    }

    /// <summary>
    /// Stores the position of a recently placed bomb.
    /// </summary>
    public void PlaceBomb(Position bombPosition)
    {
        _bombPositions.Add(bombPosition);
    }

    /// <summary>
    /// Removes a recently exploded bomb from the game board.
    /// </summary>
    /// <param name="bombPosition"></param>
    public void RemoveBomb(Position bombPosition)
    {
        _bombPositions.Remove(bombPosition);
    }

    #endregion
}