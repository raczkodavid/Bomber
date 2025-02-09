using Bomber.Model;
using Bomber.WPF.View;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

namespace Bomber.WPF.ViewModel
{
    /// <summary>
    /// ViewModel for the game, handling the interaction between the view and the model.
    /// </summary>
    class GameViewModel : ViewModelBase
    {
        #region Fields

        private readonly GameModel _model;
        private readonly MapSelectorWindow _mapSelectorWindow;
        private int _mapSize;
        private int _gameTime;
        private int _imageMargin = 5;
        private int _enemiesKilled;
        private string _pauseResumeText = "Pause";
        private readonly DispatcherTimer _timer;
        private bool _canStart = true;

        #endregion

        #region Command Properties

        // Commands
        /// <summary>
        /// Command to start a new game.
        /// </summary>
        public DelegateCommand NewGameCommand { get; private set; }

        /// <summary>
        /// Command to start the game.
        /// </summary>
        public DelegateCommand StartGameCommand { get; private set; }

        /// <summary>
        /// Command to pause or resume the game.
        /// </summary>
        public DelegateCommand PauseResumeCommand { get; private set; }

        /// <summary>
        /// Command to move the player left.
        /// </summary>
        public DelegateCommand MoveLeftCommand { get; private set; }

        /// <summary>
        /// Command to move the player right.
        /// </summary>
        public DelegateCommand MoveRightCommand { get; private set; }

        /// <summary>
        /// Command to move the player up.
        /// </summary>
        public DelegateCommand MoveUpCommand { get; private set; }

        /// <summary>
        /// Command to move the player down.
        /// </summary>
        public DelegateCommand MoveDownCommand { get; private set; }

        /// <summary>
        /// Command to place a bomb.
        /// </summary>
        public DelegateCommand PlaceBombCommand { get; private set; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the game time as a formatted string.
        /// </summary>
        public string GameTime => TimeSpan.FromSeconds(_gameTime).ToString();

        /// <summary>
        /// Gets or sets the size of the map.
        /// </summary>
        public int MapSize
        {
            get => _mapSize;
            set
            {
                _mapSize = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the text for the pause/resume button.
        /// </summary>
        public string PauseResumeText
        {
            get => _pauseResumeText;
            set
            {
                _pauseResumeText = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the number of enemies killed.
        /// </summary>
        public int EnemiesKilled
        {
            get => _enemiesKilled;
            set
            {
                _enemiesKilled = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the game can start.
        /// </summary>
        public bool CanStart
        {
            get => _canStart;
            set
            {
                _canStart = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the margin for images.
        /// </summary>
        public int ImageMargin
        {
            get => _imageMargin;
            set
            {
                _imageMargin = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the collection of game fields.
        /// </summary>
        public ObservableCollection<GameField> GameFields { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GameViewModel"/> class.
        /// </summary>
        /// <param name="model">The game model.</param>
        /// <param name="mapSelectorWindow">The map selector window.</param>
        public GameViewModel(GameModel model, MapSelectorWindow mapSelectorWindow)
        {
            _model = model;
            _timer = new DispatcherTimer();
            GameFields = new ObservableCollection<GameField>();

            _mapSelectorWindow = mapSelectorWindow;
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += OnTimerTick;

            // Connect model with ViewModel
            _model.PlayerMoved += Model_PlayerMoved;
            _model.BombExploded += Model_BombExploded;
            _model.BombPlaced += Model_BombPlaced;
            _model.EnemiesMoved += Model_EnemiesMoved;
            _model.GameEnded += Model_GameEnded;
            _model.GameBoardLoaded += Model_GameBoardLoaded;

            // Commands
            NewGameCommand = new DelegateCommand(_ => OnNewGame());
            PauseResumeCommand = new DelegateCommand(_ => OnGamePauseResume());
            StartGameCommand = new DelegateCommand(_ => OnGameStarted());
            MoveLeftCommand = new DelegateCommand(_ => _model.MovePlayer(Direction.LEFT));
            MoveRightCommand = new DelegateCommand(_ => _model.MovePlayer(Direction.RIGHT));
            MoveUpCommand = new DelegateCommand(_ => _model.MovePlayer(Direction.UP));
            MoveDownCommand = new DelegateCommand(_ => _model.MovePlayer(Direction.DOWN));
            PlaceBombCommand = new DelegateCommand(_ => _model.PlaceBomb());
        }

        #endregion

        #region Model Event Handler Methods

        /// <summary>
        /// Handles the game ended event from the model.
        /// </summary>
        private void Model_GameEnded(object? sender, GameEndedEventArgs e)
        {
            // stop the timer
            _timer.Stop();
            // if the player won, display a message box with the victory message, otherwise display a message box with the defeat message
            string message = e.IsVictory ? "Congratulations! You won!" : "Game Over! You lost!";
            MessageBoxResult result = MessageBox.Show(message, "Game Ended", MessageBoxButton.OK);

            if (result == MessageBoxResult.OK)
                OnNewGame();
        }

        /// <summary>
        /// Handles the enemies moved event from the model.
        /// </summary>
        private void Model_EnemiesMoved(object? sender, EnemiesMovedEventArgs e)
        {
            // reset the previous enemy positions, and update the new enemy positions
            for (int i = 0; i < e.PrevPositions.Count; ++i)
            {
                // reset the previous enemy position
                GameFields[e.PrevPositions[i].Y * MapSize + e.PrevPositions[i].X].FieldType = GetFieldType(e.PrevPositions[i]);

                // update the new enemy position
                GameFields[e.NewPositions[i].Y * MapSize + e.NewPositions[i].X].FieldType = FieldType.ENEMY;
            }
        }

        /// <summary>
        /// Handles the bomb exploded event from the model.
        /// </summary>
        private void Model_BombExploded(object? sender, BombExplodedEventArgs e)
        {
            // TODO : Refactor
            int startRow = Math.Max(0, e.Position.Y - e.ExplosionRadius);
            int endRow = Math.Min(MapSize - 1, e.Position.Y + e.ExplosionRadius);
            int startCol = Math.Max(0, e.Position.X - e.ExplosionRadius);
            int endCol = Math.Min(MapSize - 1, e.Position.X + e.ExplosionRadius);

            SetFieldTypesInRadius(startRow, endRow, startCol, endCol, true);

            EnemiesKilled = _model.PlayerScore;

            // after 1 second, set reset the explosion fields to their original type
            Task.Delay(1000).ContinueWith(_ =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    SetFieldTypesInRadius(startRow, endRow, startCol, endCol, false);
                });
            });
        }

        /// <summary>
        /// Handles the player moved event from the model.
        /// </summary>
        private void Model_PlayerMoved(object? sender, PlayerMovedEventArgs e)
        {
            // reset the previous player position
            GameFields[e.PreviousPosition.Y * MapSize + e.PreviousPosition.X].FieldType = GetFieldType(e.PreviousPosition);

            // update the new player position
            GameFields[e.NewPosition.Y * MapSize + e.NewPosition.X].FieldType = FieldType.PLAYER;
        }

        /// <summary>
        /// Handles the bomb placed event from the model.
        /// </summary>
        private void Model_BombPlaced(object? sender, BombPlacedEventArgs e)
        {
            // update the bomb position
            GameFields[e.BombPosition.Y * MapSize + e.BombPosition.X].FieldType = FieldType.BOMB;
        }

        /// <summary>
        /// Handles the game board loaded event from the model.
        /// </summary>
        private void Model_GameBoardLoaded(object? sender, EventArgs e)
        {
            // Initialize the GameFields collection (ObservableCollection)
            MapSize = _model.GameBoard!.Size;

            // Set the image margin based on the map size
            ImageMargin = MapSize switch
            {
                10 => 3,
                15 => 2,
                20 => 1,
                _ => 1
            };

            // Fill the GameFields collection with the field types
            for (int i = 0; i < MapSize; ++i)
            {
                for (int j = 0; j < MapSize; ++j)
                {
                    GameFields.Add(new GameField
                    {
                        FieldType = GetFieldType(new Position(i, j))
                    });
                }
            }
        }

        #endregion

        #region Event Handler Methods

        /// <summary>
        /// Handles the timer tick event.
        /// </summary>
        private void OnTimerTick(object? sender, EventArgs e)
        {
            ++_gameTime;
            OnPropertyChanged(nameof(GameTime));
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        private void OnGameStarted()
        {
            _timer.Start();
            _model.StartGame();
            StartGame?.Invoke(this, EventArgs.Empty);
            CanStart = false;
        }

        /// <summary>
        /// Pauses or resumes the game.
        /// </summary>
        private void OnGamePauseResume()
        {
            if (_model.IsGameRunning)
            {
                _timer.Stop();
                _model.PauseGame();
                PauseResumeText = "Resume";
            }
            else
            {
                _timer.Start();
                _model.ResumeGame();
                PauseResumeText = "Pause";
            }
            PauseResumeGame?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Starts a new game.
        /// </summary>
        private void OnNewGame()
        {
            _gameTime = 0;
            OnPropertyChanged(nameof(GameTime));
            _timer.Stop();
            EnemiesKilled = 0;
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                GameFields.Clear();
            });
            CanStart = true;

            // Hide this window and show the map selector window
            Application.Current.Dispatcher.Invoke(() =>
            {
                Application.Current.MainWindow!.Hide();
                Application.Current.MainWindow = _mapSelectorWindow;
                Application.Current.MainWindow.Show();
            });

            NewGame?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Loads a map from the specified path.
        /// </summary>
        /// <param name="mapPath">The path to the map file.</param>
        /// <returns>True if the map was loaded successfully, otherwise false.</returns>
        public bool LoadMap(string mapPath)
        {
            try
            {
                _model.NewGame(mapPath);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the field type for the specified position.
        /// </summary>
        /// <param name="pos">The position to get the field type for.</param>
        /// <returns>The field type at the specified position.</returns>
        private FieldType GetFieldType(Position pos)
        {
            if (pos == _model.Player.Position)
                return FieldType.PLAYER;

            if (_model.GameBoard!.IsEnemyTile(pos))
                return FieldType.ENEMY;

            if (_model.GameBoard.IsWallTile(pos))
                return FieldType.WALL;

            if (_model.GameBoard.IsBombTile(pos))
                return FieldType.BOMB;

            return FieldType.EMPTY;
        }

        /// <summary>
        /// Sets the field types in a specified radius to either explosion or their original type.
        /// </summary>
        /// <param name="startRow">The starting row of the radius.</param>
        /// <param name="endRow">The ending row of the radius.</param>
        /// <param name="startCol">The starting column of the radius.</param>
        /// <param name="endCol">The ending column of the radius.</param>
        /// <param name="toExplosion">Whether to set the fields to explosion or their original type.</param>
        private void SetFieldTypesInRadius(int startRow, int endRow, int startCol, int endCol, bool toExplosion)
        {
            for (int i = startRow; i <= endRow; ++i)
            {
                for (int j = startCol; j <= endCol; ++j)
                {
                    GameFields[i * MapSize + j].FieldType = toExplosion ? FieldType.EXPLOSION : GetFieldType(new Position(i, j));
                }
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Event raised when a new game is started.
        /// </summary>
        public event EventHandler? NewGame;
        /// <summary>
        /// Event raised when the game is started.
        /// </summary>
        public event EventHandler? StartGame;
        /// <summary>
        /// Event raised when the game is paused or resumed.
        /// </summary>
        public event EventHandler? PauseResumeGame;

        #endregion
    }
}