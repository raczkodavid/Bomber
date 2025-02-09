using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;

namespace BomberGame_Avalonia.ViewModels
{
    /// <summary>
    /// ViewModel for selecting a map in the Bomber game.
    /// </summary>
    public class MapSelectorViewModel : ViewModelBase
    {
        #region Fields

        private string _selectedMapPath = string.Empty;
        private readonly FilePicker _filePicker;

        private readonly Dictionary<string, string> _mapInformation = new()
        {
            { "easy", "avares://BomberGame_Avalonia/Assets/Maps/map10x10.txt" },
            { "medium", "avares://BomberGame_Avalonia/Assets/Maps/map15x15.txt" },
            { "hard", "avares://BomberGame_Avalonia/Assets/Maps/map20x20.txt" },
        };


        #endregion

        #region Properties

        /// <summary>
        /// Command to select a map.
        /// </summary>
        public RelayCommand<object?> SelectMapCommand { get; private set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MapSelectorViewModel"/> class.
        /// </summary>
        public MapSelectorViewModel(FilePicker filePicker)
        {
            SelectMapCommand = new RelayCommand<object?>(OnMapSelected);
            _filePicker = filePicker;
        }

        #endregion

        #region Events

        public event EventHandler<MapSelectedEventArgs>? MapSelected;

        #endregion

        #region Methods

        private async void OnMapSelected(object? param)
        {
            if (param is string mapName)
            {
                // check if the map name is in the dictionary
                if (_mapInformation.TryGetValue(mapName, out var mapPath))
                {
                    _selectedMapPath = mapPath;
                    MapSelected?.Invoke(this, new MapSelectedEventArgs(true, _selectedMapPath));
                    return;
                }

                // check if the map name is custom, if so open a file dialog
                if (mapName == "custom")
                {
                    FilePickerOpenOptions options = new()
                    {
                        Title = "Select a map file",
                        AllowMultiple = false,
                        FileTypeFilter =
                        [
                            new FilePickerFileType("Text files")
                            {
                                Patterns = ["*.txt"]
                            }
                        ]
                    };

                    string? path = await _filePicker.OpenFilePickerAsync(options);

                    if (path != null)
                    {
                        _selectedMapPath = path;
                        MapSelected?.Invoke(this, new MapSelectedEventArgs(true, _selectedMapPath, "", true));
                        return;
                    }

                    MapSelected?.Invoke(this, new MapSelectedEventArgs(false, "Please select a map!"));
                    return;
                }

            }
            MapSelected?.Invoke(this, new MapSelectedEventArgs(false, "Please select a  valid map!"));
        }

        #endregion
    }
}