using System;

namespace BomberGame_Avalonia.ViewModels
{
    public class MapSelectedEventArgs : EventArgs
    {
        #region Properties
        public bool Success { get; }
        public string ErrorMessage { get; }
        public string MapPath { get; }
        public bool IsCustom { get; }

        #endregion

        #region Constructors

        public MapSelectedEventArgs(bool success, string mapPath = "", string errorMessage = "", bool isCustom = false)
        {
            Success = success;
            MapPath = mapPath;
            ErrorMessage = errorMessage;
            IsCustom = isCustom;
        }

        #endregion
    }
}
