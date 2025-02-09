namespace Bomber.WPF.ViewModel
{
    class GameField : ViewModelBase
    {
        private FieldType _fieldType;

        public FieldType FieldType
        {
            get => _fieldType;
            set
            {
                _fieldType = value;
                OnPropertyChanged();
            }
        }
    }
}
