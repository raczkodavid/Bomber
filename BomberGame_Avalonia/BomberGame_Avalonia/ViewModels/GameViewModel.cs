using Avalonia.Threading;
using Bomber.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace BomberGame_Avalonia.ViewModels
{
    public partial class GameViewModel : ViewModelBase
    {
        #region Fields

        private readonly GameModel _model;
        private readonly DispatcherTimer _timer;

        #endregion

        #region Command Properties

        public RelayCommand NewGameCommand { get; private set; }
        public RelayCommand StartGameCommand { get; private set; }
        public RelayCommand PauseResumeCommand { get; private set; }
        public RelayCommand MoveLeftCommand { get; private set; }
        public RelayCommand MoveRightCommand { get; private set; }
        public RelayCommand MoveUpCommand { get; private set; }
        public RelayCommand MoveDownCommand { get; private set; }
        public RelayCommand PlaceBombCommand { get; private set; }

        #endregion

        #region Observable Properties

        [ObservableProperty]
        private int _mapSize;

        [ObservableProperty]
        private int _gameTime;

        [ObservableProperty]
        private int _imageMargin = 5;

        [ObservableProperty]
        private int _enemiesKilled;

        [ObservableProperty]
        private string _pauseResumeText = "Pause";

        [ObservableProperty]
        private bool _canStart = true;

        public ObservableCollection<GameField> GameFields { get; private set; }

        #endregion

        #region Constructors

        public GameViewModel(GameModel model)
        {
            _model = model;
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += OnTimerTick;
            GameFields = new();

            // Model Event Handlers
            _model.PlayerMoved += Model_PlayerMoved;
            _model.BombExploded += Model_BombExploded;
            _model.BombPlaced += Model_BombPlaced;
            _model.EnemiesMoved += Model_EnemiesMoved;
            _model.GameEnded += Model_GameEnded;
            _model.GameBoardLoaded += Model_GameBoardLoaded;

            // Commands
            NewGameCommand = new RelayCommand(OnNewGame);
            PauseResumeCommand = new RelayCommand(OnGamePauseResume);
            StartGameCommand = new RelayCommand(OnGameStarted);
            MoveLeftCommand = new RelayCommand(() => _model.MovePlayer(Direction.LEFT));
            MoveRightCommand = new RelayCommand(() => _model.MovePlayer(Direction.RIGHT));
            MoveUpCommand = new RelayCommand(() => _model.MovePlayer(Direction.UP));
            MoveDownCommand = new RelayCommand(() => _model.MovePlayer(Direction.DOWN));
            PlaceBombCommand = new RelayCommand(() => _model.PlaceBomb());
        }

        #endregion

        #region Model Event Handler Methods

        private void Model_GameEnded(object? sender, GameEndedEventArgs e)
        {
            _timer.Stop();
            EndGame?.Invoke(this, e);
        }

        private void Model_EnemiesMoved(object? sender, EnemiesMovedEventArgs e)
        {
            for (int i = 0; i < e.PrevPositions.Count; ++i)
            {
                GameFields[e.PrevPositions[i].Y * MapSize + e.PrevPositions[i].X].FieldType = GetFieldType(e.PrevPositions[i]);
                GameFields[e.NewPositions[i].Y * MapSize + e.NewPositions[i].X].FieldType = FieldType.ENEMY;
            }
        }

        private void Model_BombExploded(object? sender, BombExplodedEventArgs e)
        {
            int startRow = Math.Max(0, e.Position.Y - e.ExplosionRadius);
            int endRow = Math.Min(MapSize - 1, e.Position.Y + e.ExplosionRadius);
            int startCol = Math.Max(0, e.Position.X - e.ExplosionRadius);
            int endCol = Math.Min(MapSize - 1, e.Position.X + e.ExplosionRadius);

            SetFieldTypesInRadius(startRow, endRow, startCol, endCol, true);

            EnemiesKilled = _model.PlayerScore;

            Task.Delay(1000).ContinueWith(_ =>
            {
                Dispatcher.UIThread.Post(() =>
                {
                    SetFieldTypesInRadius(startRow, endRow, startCol, endCol, false);
                });
            });
        }

        private void Model_PlayerMoved(object? sender, PlayerMovedEventArgs e)
        {
            GameFields[e.PreviousPosition.Y * MapSize + e.PreviousPosition.X].FieldType = GetFieldType(e.PreviousPosition);
            GameFields[e.NewPosition.Y * MapSize + e.NewPosition.X].FieldType = FieldType.PLAYER;
        }

        private void Model_BombPlaced(object? sender, BombPlacedEventArgs e)
        {
            GameFields[e.BombPosition.Y * MapSize + e.BombPosition.X].FieldType = FieldType.BOMB;
        }

        private void Model_GameBoardLoaded(object? sender, EventArgs e)
        {
            MapSize = _model.GameBoard!.Size;
            ImageMargin = MapSize switch
            {
                10 => 3,
                15 => 2,
                20 => 1,
                _ => 1
            };

            GameFields.Clear();
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

        private void OnTimerTick(object? sender, EventArgs e)
        {
            ++GameTime;
        }

        private void OnGameStarted()
        {
            if (!CanStart)
                return;

            _timer.Start();
            _model.StartGame();
            CanStart = false;
        }

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
        }

        private void OnNewGame()
        {
            GameTime = 0;
            _timer.Stop();
            EnemiesKilled = 0;
            GameFields.Clear();
            CanStart = true;

            NewGame?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Helper Methods

        public bool LoadMap(Stream stream)
        {
            GameTime = 0;
            _timer.Stop();
            EnemiesKilled = 0;
            GameFields.Clear();
            CanStart = true;
            try
            {
                _model.NewGame(stream);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool LoadMap(string mapPath)
        {
            FileStream fs = new FileStream(mapPath, FileMode.Open);
            return LoadMap(fs);
        }

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

        public event EventHandler? NewGame;
        public event EventHandler<GameEndedEventArgs>? EndGame;

        #endregion
    }
}
