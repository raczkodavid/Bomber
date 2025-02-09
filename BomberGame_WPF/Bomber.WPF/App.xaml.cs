using Bomber.WPF.View;
using Bomber.WPF.ViewModel;
using System.Windows;

namespace Bomber.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields

        private MapSelectorViewModel _mapSelectorViewModel = null!;
        private MapSelectorWindow _mapSelectorWindow = null!;

        #endregion

        #region Constructors

        public App()
        {
            Startup += App_Startup;
        }

        #endregion

        #region Application event handlers

        private void App_Startup(object sender, StartupEventArgs e)
        {
            // Create MapSelector ViewModel
            _mapSelectorWindow = new MapSelectorWindow();
            _mapSelectorViewModel = new MapSelectorViewModel(_mapSelectorWindow);
            _mapSelectorWindow.DataContext = _mapSelectorViewModel;
            _mapSelectorWindow.Show();
        }

        #endregion
    }

}
