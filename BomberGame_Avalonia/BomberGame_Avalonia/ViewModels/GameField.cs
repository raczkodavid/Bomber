using CommunityToolkit.Mvvm.ComponentModel;

namespace BomberGame_Avalonia.ViewModels
{
    public partial class GameField : ViewModelBase
    {
        [ObservableProperty]
        private FieldType _fieldType;
    }
}