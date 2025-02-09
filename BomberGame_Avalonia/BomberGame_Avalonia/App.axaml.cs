using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Avalonia.Threading;
using Bomber.Model;
using Bomber.Persistence;
using BomberGame_Avalonia.ViewModels;
using BomberGame_Avalonia.Views;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Threading.Tasks;

namespace BomberGame_Avalonia;

public partial class App : Application
{
    #region Fields

    // ViewModels
    private MapSelectorViewModel? _mapSelectorViewModel;
    private GameViewModel? _gameViewModel;

    // Windows
    private MapSelectorWindow? _mapSelectorWindow;
    private GameWindow? _gameWindow;
    private MapSelectorView? _mapSelectorView;
    private GameView? _gameView;

    #endregion

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        _mapSelectorViewModel = new MapSelectorViewModel(new FilePicker());
        _mapSelectorViewModel.MapSelected += OnMapSelected;

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            _mapSelectorWindow = new MapSelectorWindow()
            {
                DataContext = _mapSelectorViewModel
            };

            desktop.MainWindow = _mapSelectorWindow;
        }

        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            _mapSelectorView = new MapSelectorView()
            {
                DataContext = _mapSelectorViewModel
            };

            singleViewPlatform.MainView = _mapSelectorView;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private async void OnMapSelected(object? sender, MapSelectedEventArgs e)
    {
        // Display error message there was an error selecting the map
        if (!e.Success)
        {
            await MessageBoxManager.GetMessageBoxStandard(
                    "Error",
                    e.ErrorMessage,
                    ButtonEnum.Ok, Icon.Error)
                .ShowAsync();
            return;
        }

        if (_gameViewModel is null)
        {
            _gameViewModel = new GameViewModel(new GameModel(new BomberDataAccess()));
            _gameViewModel.NewGame += (_, _) => ViewModel_NewGame();
            _gameViewModel.EndGame += async (_, args) => await ViewModel_GameEnded(args);

        }

        if (e.IsCustom ? !_gameViewModel.LoadMap(e.MapPath) : !_gameViewModel.LoadMap(AssetLoader.Open(new Uri(e.MapPath))))
        {
            await MessageBoxManager.GetMessageBoxStandard(
                    "Error",
                    "Map could not be loaded!",
                    ButtonEnum.Ok, Icon.Error)
                .ShowAsync();
            return;
        }

        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime)
            {
                _gameWindow = new GameWindow()
                {
                    DataContext = _gameViewModel
                };

                _gameWindow.Show();
                _mapSelectorWindow!.Hide();
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                _gameView = new GameView()
                {
                    DataContext = _gameViewModel
                };

                singleViewPlatform.MainView = _gameView;
            }
        });
    }

    private async Task ViewModel_GameEnded(GameEndedEventArgs gameEndedEventArgs)
    {
        await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var messageBoxResult = await MessageBoxManager.GetMessageBoxStandard(
                    "Game Over",
                    $"Game Over! You {(gameEndedEventArgs.IsVictory ? "won" : "lost")}.",
                    ButtonEnum.Ok, Icon.Info)
                .ShowAsync();

            if (messageBoxResult == ButtonResult.Ok)
            {
                // Close the game window
                if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime)
                {
                    _gameWindow!.Close();
                    _mapSelectorWindow!.Show();
                }
                else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
                    singleViewPlatform.MainView = _mapSelectorView;
            }

        });
    }

    private void ViewModel_NewGame()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime)
        {
            _gameWindow!.Close();
            _mapSelectorWindow!.Show();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            singleViewPlatform.MainView = _mapSelectorView;
    }
}
