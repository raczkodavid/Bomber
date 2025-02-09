using BomberModel;
using BomberPersistence;

using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BomberUI
{
    public partial class GameForm : Form
    {
        #region Private Fields

        /// <summary>
        /// The Core of the game
        /// </summary>
        private readonly GameModel _gameModel;

        /// <summary>
        /// PictureBox matrix representing the game field.
        /// </summary>
        private PictureBox[,] _gameField = null!;

        /// <summary>
        /// Timer for tracking game time.
        /// </summary>
        private readonly Timer _timer;

        /// <summary>
        /// Elapsed seconds since the game started.
        /// </summary>
        private int _elapsedSeconds;

        /// <summary>
        /// Indicates if the game has been started before.
        /// </summary>
        private bool _wasStartedBefore;

        /// <summary>
        /// Indicates if the form is closing.
        /// </summary>
        public bool IsClosing { get; private set; }

        // Reference to MapSelect form
        private readonly MapSelect _mapSelectForm;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GameForm"/> class.
        /// </summary>
        /// <param name="mapSelectForm">The reference to the MapSelect form.</param>
        public GameForm(MapSelect mapSelectForm)
        {
            InitializeComponent();

            _mapSelectForm = mapSelectForm;
            IBomberDataAccess dataAccess = new BomberDataAccess();
            _gameModel = new GameModel(dataAccess);
            _timer = new Timer { Interval = 1000 };

            // Subscribe to model events
            _timer.Tick += OnTimerClick;
            _gameModel.GameBoardLoaded += OnGameBoardLoaded;
            _gameModel.PlayerMoved += OnPlayerMoved;
            _gameModel.EnemiesMoved += OnEnemiesMoved;
            _gameModel.GameEnded += (_, e) => OnGameEnded(e);
            _gameModel.BombExploded += (_, e) => OnBombExploded(e);

            // Subscribe to form events
            pauseGameBtn.Click += OnGamePaused;
            newGameBtn.Click += OnNewGame;
            startGameBtn.Click += OnGameStarted;

            // Enable key preview for handling keyboard input
            KeyPreview = true;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads a map into the game from the specified path.
        /// </summary>
        /// <param name="mapPath">The path to the map file.</param>
        public bool LoadMap(string mapPath)
        {
            // Try to load in the map, if it fails for some reason, return false
            // The boolean value will be used by the MapSelect Form to determine whether the loading was successful
            try
            {
                _gameModel.NewGame(mapPath);
            }
            catch
            {
                return false;
            }

            // Ensure timer is stopped
            _timer.Stop();
            _wasStartedBefore = false;

            // Reset label and button texts
            _elapsedSeconds = 0;
            displayTimeLabel.Text = ConvertSeconds(_elapsedSeconds);
            killedEnemiesCountLabel.Text = "0";
            startGameBtn.Text = "Start";

            // Enable key down events for player movement
            KeyDown += OnKeyDown;

            return true;
        }

        #endregion

        #region Event Handlers

        // Event handler for the timer tick (used for game time)
        private void OnTimerClick(object? sender, EventArgs e)
        {
            _elapsedSeconds++;
            displayTimeLabel.Text = ConvertSeconds(_elapsedSeconds);
        }

        // Handle when the game board is loaded
        private void OnGameBoardLoaded(object? sender, EventArgs e)
        {
            // Clear the existing PictureBoxes
            gridPanel.Controls.Clear();

            int pictureSize = gridPanel.Height / _gameModel.GameBoard!.Size;
            _gameField = new PictureBox[_gameModel.GameBoard.Size, _gameModel.GameBoard.Size];
            int paddingSize = (pictureSize / _gameModel.GameBoard.Size);

            for (int i = 0; i < _gameModel.GameBoard.Size; i++)
            {
                for (int j = 0; j < _gameModel.GameBoard.Size; j++)
                {
                    _gameField[i, j] = new PictureBox
                    {
                        Size = new Size(pictureSize - paddingSize, pictureSize - paddingSize),
                        Location = new Point(pictureSize * j, pictureSize * i),
                        BackColor = GetTileColor(new Position(i, j)),
                        BorderStyle = BorderStyle.FixedSingle,
                    };
                    _gameField[i, j].SizeMode = PictureBoxSizeMode.StretchImage;

                    gridPanel.Controls.Add(_gameField[i, j]);
                }
            }
        }

        // Handle player movement
        private void OnPlayerMoved(object? sender, PlayerMovedEventArgs e)
        {
            _gameField[e.PreviousPosition.Y, e.PreviousPosition.X].BackColor = GetTileColor(e.PreviousPosition);
            _gameField[e.NewPosition.Y, e.NewPosition.X].BackColor = Color.Blue;
        }

        // Handle enemy movement
        private void OnEnemiesMoved(object? sender, EnemiesMovedEventArgs e)
        {
            for (int i = 0; i < e.PrevPositions.Count; i++)
            {
                _gameField[e.PrevPositions[i].Y, e.PrevPositions[i].X].BackColor = GetTileColor(e.PrevPositions[i]);
                _gameField[e.NewPositions[i].Y, e.NewPositions[i].X].BackColor = Color.Red;
            }
        }

        // Handle bomb explosion
        private void OnBombExploded(BombExplodedEventArgs e)
        {
            int upperBound = Math.Max(0, e.Position.Y - e.ExplosionRadius);
            int leftBound = Math.Max(0, e.Position.X - e.ExplosionRadius);

            int lowerBound = Math.Min(_gameModel.GameBoard!.Size - 1, e.Position.Y + e.ExplosionRadius);
            int rightBound = Math.Min(_gameModel.GameBoard.Size - 1, e.Position.X + e.ExplosionRadius);

            killedEnemiesCountLabel.Text = _gameModel.PlayerScore.ToString();

            for (int i = upperBound; i <= lowerBound; i++)
            {
                for (int j = leftBound; j <= rightBound; j++)
                {
                    _gameField[i, j].BackColor = Color.Orange;
                }
            }

            // Revert the colors after a delay
            Task.Delay(1000).ContinueWith(_ =>
            {
                RevertTileColors(upperBound, lowerBound, leftBound, rightBound);
            });
        }

        private void RevertTileColors(int upperBound, int lowerBound, int leftBound, int rightBound)
        {
            for (int i = upperBound; i <= lowerBound; i++)
            {
                for (int j = leftBound; j <= rightBound; j++)
                {
                    _gameField[i, j].BackColor = GetTileColor(new Position(i, j));
                }
            }
        }

        // Handle when the game ends
        private void OnGameEnded(GameEndedEventArgs e)
        {
            _timer.Stop();
            KeyDown -= OnKeyDown;

            MessageBox.Show(e.IsVictory
                ? $"Congratulations! You have won the game in {_elapsedSeconds} seconds!"
                : $"Game over! You have lost the game. You have killed {_gameModel.PlayerScore} enemies.");
        }

        // Start / resume the game
        private void OnGameStarted(object? sender, EventArgs e)
        {
            if (!_wasStartedBefore)
            {
                _timer.Start();
                _gameModel.StartGame();
                startGameBtn.Text = "Resume";
                _wasStartedBefore = true;
            }
            else
            {
                if (_gameModel.IsGameRunning) return;
                _timer.Start();
                _gameModel.ResumeGame();
            }
        }

        // Pause the game
        private void OnGamePaused(object? sender, EventArgs e)
        {
            if (!_gameModel.IsGameRunning) return;
            _gameModel.PauseGame();
            _timer.Stop();
        }

        // Start a new game (back to map selection)
        private void OnNewGame(object? sender, EventArgs e)
        {
            if (_gameModel.IsGameRunning)
            {
                MessageBox.Show("A game is already running. Please finish it before starting a new one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Hide GameForm and return to MapSelect form
            Hide();
            KeyDown -= OnKeyDown;
            _mapSelectForm.Show();
        }

        // Handle key presses for player movement
        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    _gameModel.MovePlayer(Direction.UP);
                    break;
                case Keys.A:
                    _gameModel.MovePlayer(Direction.LEFT);
                    break;
                case Keys.S:
                    _gameModel.MovePlayer(Direction.DOWN);
                    break;
                case Keys.D:
                    _gameModel.MovePlayer(Direction.RIGHT);
                    break;
                case Keys.B:
                    _gameModel.PlaceBomb();
                    break;
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Converts seconds to an HH:mm:ss format string.
        /// </summary>
        /// <param name="seconds">The number of seconds to convert.</param>
        /// <returns>A string representing the time in HH:mm:ss format.</returns>
        private string ConvertSeconds(int seconds)
        {
            int hours = seconds / 3600;
            int minutes = (seconds % 3600) / 60;
            int secs = seconds % 60;

            return $"{hours:D2}:{minutes:D2}:{secs:D2}";
        }

        /// <summary>
        /// Gets the color for a specific tile based on its position.
        /// </summary>
        /// <param name="pos">The position of the tile.</param>
        /// <returns>The color of the tile.</returns>
        private Color GetTileColor(Position pos)
        {
            Color color;
            if (pos == _gameModel.Player.Position) color = Color.Blue;
            else if (_gameModel.GameBoard!.IsEnemyTile(pos)) color = Color.Red;
            else if (_gameModel.GameBoard.IsWallTile(pos)) color = Color.Black;
            else if (_gameModel.GameBoard.IsBombTile(pos)) color = Color.Gray;
            else color = Color.Green;

            return color;
        }

        #endregion

        #region Form Lifecycle

        /// <summary>
        /// Handles the form closed event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            IsClosing = true;
            base.OnFormClosed(e); // Call base method first

            // Close the MapSelect form if it exists
            if (!_mapSelectForm.IsClosing)
                _mapSelectForm.Close();
        }

        #endregion
    }
}
