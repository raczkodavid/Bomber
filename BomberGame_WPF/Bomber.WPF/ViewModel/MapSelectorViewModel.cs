using Bomber.Model;
using Bomber.Persistence;
using Bomber.WPF.View;
using Microsoft.Win32;
using System.Windows;

namespace Bomber.WPF.ViewModel
{
    /// <summary>
    /// ViewModel for selecting a map in the Bomber game.
    /// </summary>
    class MapSelectorViewModel : ViewModelBase
    {
        #region Fields

        private string _selectedMapPath = string.Empty;
        private GameWindow? _gameWindow;
        private GameViewModel? _gameViewModel;
        private readonly MapSelectorWindow _window;

        private readonly Dictionary<string, string> _mapInformaton = new()
            {
                {"easy",    "Persistence/Maps/map10x10.txt" },
                {"medium",  "Persistence/Maps/map15x15.txt" },
                {"hard",    "Persistence/Maps/map20x20.txt" },
            };

        #endregion

        #region Properties
        /// <summary>
        /// Command to select a map.
        /// </summary>
        public DelegateCommand SelectMapCommand { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MapSelectorViewModel"/> class.
        /// </summary>
        /// <param name="window">The map selector window.</param>
        public MapSelectorViewModel(MapSelectorWindow window)
        {
            _window = window;
            SelectMapCommand = new DelegateCommand(OnMapSelected);
        }

        #endregion

        #region Methods
        /// <summary>
        /// Handles the map selection logic.
        /// </summary>
        /// <param name="param">The parameter indicating which map to select.</param>
        private void OnMapSelected(object? param)
        {
            if (param is string buttonName)
            {
                if (_mapInformaton.TryGetValue(buttonName, out var value))
                    _selectedMapPath = value;

                else if (buttonName == "custom")
                {
                    // Open file dialog for custom map selection
                    var fileDialog = new OpenFileDialog
                    {
                        Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                        Title = "Select a Map File"
                    };

                    bool? result = fileDialog.ShowDialog();
                    if (result == true)
                        _selectedMapPath = fileDialog.FileName;
                }

                // If no map is selected, return
                else
                    return;
            }

            // If the parameter is not valid, return
            else
                return;

            if (_gameViewModel == null)
            {
                IBomberDataAccess bomberDataAccess = new BomberDataAccess();
                GameModel gameModel = new GameModel(bomberDataAccess);
                _gameViewModel = new GameViewModel(gameModel, _window);
                _gameWindow = new GameWindow
                {
                    DataContext = _gameViewModel
                };
            }

            if (!_gameViewModel.LoadMap(_selectedMapPath))
                MessageBox.Show("Error loading map", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            else
                OpenGameWindow();
        }

        /// <summary>
        /// Opens the game window and hides the map selector window.
        /// </summary>
        private void OpenGameWindow()
        {
            Application.Current.MainWindow = _gameWindow;
            _gameWindow!.Show();
            _window.Hide();
        }

        #endregion
    }
}
