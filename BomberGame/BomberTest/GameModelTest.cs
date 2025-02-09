using BomberModel;
using BomberPersistence;
using Moq;

namespace BomberTest;

[TestClass]
public class GameModelTest : IDisposable
{
    private GameModel _model = null!;
    private Mock<IBomberDataAccess> _dataAccessMock = null!;

    [TestInitialize]
    public void Initialize()
    {
        // create mock game board - 10x10 with 1 player and 1 enemy
        TileTypeDto[,] tiles = new TileTypeDto[10, 10];
        // initialize all tiles to empty
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                tiles[i, j] = new TileTypeDto(0);
            }
        }

        // put down some walls - 6th col, 7th row
        for (int i = 0; i < 10; i++)
        {
            tiles[i, 5].Value = 1;
            tiles[6, i].Value = 1;
        }

        // enemies - 2 enemies at (5, 0) and (5, 3), facing up
        List<EnemyDto> enemies =
        [
            new EnemyDto(5, 0, new DirectionDto(0)),
            new EnemyDto(5, 3, new DirectionDto(0))
        ];

        // set up mock data access to always return this game board
        GameBoardDto board = new GameBoardDto(tiles, enemies);
        _dataAccessMock = new Mock<IBomberDataAccess>();
        _dataAccessMock.Setup(d => d.LoadGameBoard(It.IsAny<String>())).Returns(board);

        // create the model
        _model = new GameModel(_dataAccessMock.Object);
    }

    [TestMethod]
    public void TestNewGame()
    {
        // doesn't matter what the path is, we're mocking it
        _model.NewGame("testMap.txt");

        // check GameBoard (size, layout, enemies)
        Assert.AreEqual(10, _model.GameBoard!.Size);

        // check player position
        Assert.AreEqual(new Position(0, 0), _model.Player.Position);

        // check if player is alive
        Assert.IsTrue(_model.Player.IsAlive);

        // check score
        Assert.AreEqual(0, _model.PlayerScore);

        // check game running
        Assert.IsFalse(_model.IsGameRunning);

        // check if map layout is correct
        for (int i = 0; i < 10; ++i)
        {
            bool isWallRow = (i == 6);
            for (int j = 0; j < 10; ++j)
            {
                if (isWallRow || j == 5) // walls
                    Assert.IsTrue(_model.GameBoard!.IsWallTile(new Position(i, j)));
                else
                    Assert.IsFalse(_model.GameBoard!.IsWallTile(new Position(i, j)));
            }
        }

        // check enemies
        Assert.AreEqual(2, _model.GameBoard!.RemainingEnemies);

        // check enemy positions
        Assert.IsTrue(_model.GameBoard!.IsEnemyTile(new Position(5, 0)));
        Assert.IsTrue(_model.GameBoard!.IsEnemyTile(new Position(5, 3)));
    }

    [TestMethod]
    public void TestPauseResumeStart()
    {
        // the game is not started yet, pause and resume has no effect
        _model.PauseGame();
        Assert.IsFalse(_model.IsGameRunning);

        _model.ResumeGame();
        Assert.IsFalse(_model.IsGameRunning);

        // Start the game
        _model.NewGame("testMap.txt");
        _model.StartGame();

        Assert.IsTrue(_model.IsGameRunning);

        _model.PauseGame();
        Assert.IsFalse(_model.IsGameRunning);

        _model.ResumeGame();
        Assert.IsTrue(_model.IsGameRunning);
    }

    [TestMethod]
    public void TestPlayerMovement()
    {
        _model = new GameModel(_dataAccessMock.Object);
        _model.NewGame("testMap.txt");

        // Game is not started, we can't move the player
        _model.MovePlayer(Direction.RIGHT); // Would land on (0, 1), but the game is not running
        Assert.AreEqual(new Position(0, 0), _model.Player.Position);

        // Start the game
        _model.StartGame();

        // Move the player UP - out of bounds
        _model.MovePlayer(Direction.UP);
        Assert.AreEqual(new Position(0, 0), _model.Player.Position);

        // Move the player RIGHT - valid
        _model.MovePlayer(Direction.RIGHT);
        Assert.AreEqual(new Position(0, 1), _model.Player.Position);

        // Move the player down 8 times - a wall is in the way (6th row)
        for (int i = 0; i < 8; ++i)
            _model.MovePlayer(Direction.DOWN);

        Assert.AreEqual(new Position(5, 1), _model.Player.Position);
    }

    [TestMethod]
    public void TestBombPlacement()
    {
        _model.NewGame("testMap.txt");
        bool isBombPlacedEventTriggered = false;
        _model.BombPlaced += (_, _) => isBombPlacedEventTriggered = true;

        // Would place a bomb at (0, 0), but the game is not running
        _model.PlaceBomb();
        Assert.IsFalse(_model.GameBoard!.IsBombTile(new Position(0, 0)));

        // Start the game
        _model.StartGame();

        // Place a bomb
        _model.PlaceBomb();
        Assert.IsTrue(_model.GameBoard!.IsBombTile(new Position(0, 0)));
        Assert.IsTrue(isBombPlacedEventTriggered);

        // Move the player to (0, 1) and place a bomb
        _model.MovePlayer(Direction.RIGHT);
        _model.PlaceBomb();
        Assert.IsTrue(_model.GameBoard!.IsBombTile(new Position(0, 1)));
    }

    [TestMethod]
    public void TestBombExplosion()
    {
        // check if the bomb explodes
        bool isBombExplodedEventTriggered = false;

        _model.NewGame("testMap.txt");
        _model.BombExploded += (_, _) => isBombExplodedEventTriggered = true;
        _model.StartGame();
        _model.MovePlayer(Direction.RIGHT);

        _model.PlaceBomb();

        // wait for the bomb to explode
        Thread.Sleep(3100);

        Assert.IsTrue(isBombExplodedEventTriggered);

        // check if the bomb is removed from the game board
        Assert.IsFalse(_model.GameBoard!.IsBombTile(new Position(0, 1)));

        // both enemies should be killed
        Assert.AreEqual(0, _model.GameBoard!.RemainingEnemies);

        // check if the score is updated
        Assert.AreEqual(2, _model.PlayerScore);

        // The game should not be running because enemies and the player died
        Assert.IsFalse(_model.IsGameRunning);
    }

    [TestMethod]
    public void TestEnemyMovement()
    {
        bool isEneiesMovedEventTriggered = false;
        _model.NewGame("testMap.txt");
        _model.EnemiesMoved += (_, _) => isEneiesMovedEventTriggered = true;

        _model.StartGame();
        // wait 1 second so enemies move
        Thread.Sleep(1100);

        Assert.IsTrue(isEneiesMovedEventTriggered);

        // both enemies should be one tile higher than their starting position
        Assert.IsTrue(_model.GameBoard!.IsEnemyTile(new Position(4, 0)));
        Assert.IsTrue(_model.GameBoard!.IsEnemyTile(new Position(4, 3)));
    }

    public void Dispose() => _model.Dispose();
}