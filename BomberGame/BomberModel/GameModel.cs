using BomberPersistence;
using System;
using System.Collections.Generic;
using System.Timers;

namespace BomberModel;

public class GameModel : IDisposable
{
    #region Private Fields

    /// <summary>
    /// Timer that controls enemy movement.
    /// </summary>
    private Timer _enemyMoveTimer;

    /// <summary>
    /// List of bombs that are currently active in the game.
    /// </summary>
    private readonly List<Bomb> _activeBombs;

    /// <summary>
    /// Interface for loading game boards from files.
    /// </summary>
    private readonly IBomberDataAccess _dataAccess;

    #endregion

    #region Public Properties

    /// <summary>
    /// Player's current score, representing the number of enemies killed.
    /// </summary>
    public int PlayerScore { get; private set; }

    /// <summary>
    /// Gets whether the game is currently running.
    /// </summary>
    public bool IsGameRunning { get; private set; }

    /// <summary>
    /// Represents the current game board.
    /// </summary>
    public GameBoard? GameBoard { get; private set; }

    /// <summary>
    /// Represents the player in the game.
    /// </summary>
    public Player Player { get; private set; }

    #endregion

    #region Events

    /// <summary>
    /// Triggered when the player has moved.
    /// </summary>
    public event EventHandler<PlayerMovedEventArgs>? PlayerMoved;

    /// <summary>
    /// Triggered when a bomb has been placed.
    /// </summary>
    public event EventHandler<BombPlacedEventArgs>? BombPlaced;

    /// <summary>
    /// Triggered when a bomb has exploded.
    /// </summary>
    public event EventHandler<BombExplodedEventArgs>? BombExploded;

    /// <summary>
    /// Triggered when the game has ended, either in victory or defeat.
    /// </summary>
    public event EventHandler<GameEndedEventArgs>? GameEnded;

    /// <summary>
    /// Triggered when the game board has successfully been loaded.
    /// </summary>
    public event EventHandler? GameBoardLoaded;

    /// <summary>
    /// Triggered when enemies have moved.
    /// </summary>
    public event EventHandler<EnemiesMovedEventArgs>? EnemiesMoved;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="GameModel"/> class.
    /// </summary>
    /// <param name="dataAccess">Data access interface for loading game boards.</param>
    public GameModel(IBomberDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
        Player = new Player();
        _activeBombs = new List<Bomb>();
        // Move enemies every second
        _enemyMoveTimer = new Timer(1000);
        _enemyMoveTimer.AutoReset = true;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Moves the player in the specified direction if the move is valid, otherwise does nothing.
    /// </summary>
    /// <param name="dir">The direction in which to move the player.</param>
    public void MovePlayer(Direction dir)
    {
        // Cannot move the player if the game is not running
        if (!IsGameRunning)
            return;

        if (!GameBoard!.CanMoveTo(Player.Position + Position.GetMovementVector(dir)))
            return;

        Position previousPos = Player.Position;
        Player.Move(dir);

        OnPlayerMoved(previousPos, Player.Position);

        if (GameBoard.IsEnemyTile(Player.Position))
            OnGameOver(false);
    }

    /// <summary>
    /// Places a bomb at the player's current position.
    /// </summary>
    public void PlaceBomb()
    {
        // Bombs cannot be placed if the game is not running
        if (!IsGameRunning)
            return;

        Bomb placedBomb = Player.PlaceBomb();
        _activeBombs.Add(placedBomb);
        GameBoard!.PlaceBomb(placedBomb.Position);
        placedBomb.BombExploded += OnBombExploded;
        OnBombPlaced(placedBomb.Position);
    }

    /// <summary>
    /// Creates a new game with the specified map.
    /// </summary>
    /// <param name="mapPath">The path to the game map file.</param>
    public void NewGame(string mapPath)
    {
        // The game must be ended or paused to start a new game
        if (IsGameRunning)
            return;

        Player = new Player();
        _activeBombs.Clear();
        PlayerScore = 0;
        LoadGameBoard(mapPath);

        _enemyMoveTimer = new Timer(1000);
        _enemyMoveTimer.Elapsed += (_, _) => OnEnemiesMoved();

    }

    /// <summary>
    /// Starts the game by activating the enemy movement timer, making enemies move.
    /// </summary>
    public void StartGame()
    {
        // If the game is already running or the game board has not been loaded properly, do nothing
        if (IsGameRunning || GameBoard == null)
            return;

        _enemyMoveTimer.Start();
        IsGameRunning = true;
    }

    /// <summary>
    /// Pauses the game by stopping the timers for enemies and bombs.
    /// </summary>
    public void PauseGame()
    {
        // The game cannot be paused if it is not running
        if (!IsGameRunning)
            return;

        _enemyMoveTimer.Stop();
        _activeBombs.ForEach(b => b.PauseCountdown());
        IsGameRunning = false;
    }

    /// <summary>
    /// Resumes the game by resuming the timers for enemies and bombs.
    /// </summary>
    public void ResumeGame()
    {
        // The game cannot be resumed if it is already running or the game board has not been loaded
        if (IsGameRunning || GameBoard == null)
            return;

        _enemyMoveTimer.Start();
        _activeBombs.ForEach(b => b.ResumeCountdown());
        IsGameRunning = true;
    }

    /// <summary>
    /// Releases all resources used by the <see cref="GameModel"/>.
    /// </summary>
    public void Dispose()
    {
        _enemyMoveTimer.Stop();
        _enemyMoveTimer.Dispose();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Loads the game board from the specified file.
    /// </summary>
    /// <param name="path">The path to the game board file.</param>
    private void LoadGameBoard(string path)
    {
        GameBoardDto gameBoardDto = _dataAccess.LoadGameBoard(path);
        int mapSize = gameBoardDto.MapLayout.GetLength(0);
        TileType[,] mapLayout = new TileType[mapSize, mapSize];
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                mapLayout[i, j] = (TileType)gameBoardDto.MapLayout[i, j].Value;
            }
        }

        List<Enemy> enemies = new List<Enemy>();
        foreach (EnemyDto enemyDto in gameBoardDto.Enemies)
        {
            enemies.Add(new Enemy(new Position(enemyDto.PositionY, enemyDto.PositionX),
                                 (Direction)enemyDto.CurrentDirection.Value));
        }

        GameBoard = new GameBoard(mapLayout, enemies);
        OnGameBoardLoaded();
    }

    /// <summary>
    /// Handles the logic when a bomb explodes.
    /// </summary>
    /// <param name="sender">The bomb that exploded.</param>
    /// <param name="e">Event arguments containing explosion details.</param>
    private void OnBombExploded(object? sender, BombExplodedEventArgs e)
    {
        if (!IsGameRunning)
            return;

        int killedEnemiesCount = GameBoard!.KillEnemiesInRadius(e.Position, e.ExplosionRadius);

        if (killedEnemiesCount > 0)
            PlayerScore += killedEnemiesCount;

        Bomb b = (Bomb)sender!;
        _activeBombs.Remove(b);
        GameBoard.RemoveBomb(b.Position);

        BombExploded?.Invoke(this, e);

        if (GameBoard!.RemainingEnemies == 0)
        {
            OnGameOver(true);
            return;
        }

        if (GameBoard!.IsEntityInRadius(Player.Position, e.Position, e.ExplosionRadius))
        {
            Player.IsAlive = false;
            OnGameOver(false);
        }
    }

    #endregion

    #region Event Triggers

    /// <summary>
    /// Raises the <see cref="PlayerMoved"/> event.s
    /// </summary>
    private void OnPlayerMoved(Position previousPosition, Position newPosition)
    {
        PlayerMoved?.Invoke(this, new PlayerMovedEventArgs(previousPosition, newPosition));
    }

    /// <summary>
    /// Raises the <see cref="BombPlaced"/> event.
    /// </summary>
    private void OnBombPlaced(Position bombPosition)
    {
        BombPlaced?.Invoke(this, new BombPlacedEventArgs(bombPosition));
    }

    /// 
    /// <summary>
    /// Raises the <see cref="GameEnded"/> event.
    /// </summary>
    private void OnGameOver(bool isVictory)
    {
        // Do not fire the event if the game is not running
        if (!IsGameRunning)
            return;

        _enemyMoveTimer.Stop();
        _enemyMoveTimer.Dispose();

        _activeBombs.ForEach(b => b.PauseCountdown());

        IsGameRunning = false;
        GameEnded?.Invoke(this, new GameEndedEventArgs(isVictory));
    }

    /// <summary>
    /// Raises the <see cref="EnemiesMoved"/> event.
    /// </summary>
    private void OnEnemiesMoved()
    {
        (List<Position> prevPositions, List<Position> newPositions) = GameBoard!.MoveEnemies();
        EnemiesMoved?.Invoke(this, new EnemiesMovedEventArgs(prevPositions, newPositions));

        if (GameBoard.IsEnemyTile(Player.Position))
            OnGameOver(false);
    }

    /// <summary>
    /// Raises the <see cref="GameBoardLoaded"/> event.
    /// </summary>
    private void OnGameBoardLoaded()
    {
        GameBoardLoaded?.Invoke(this, EventArgs.Empty);
    }

    #endregion
}