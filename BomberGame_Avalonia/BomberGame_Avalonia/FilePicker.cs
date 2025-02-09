using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BomberGame_Avalonia
{
    public class FilePicker
    {
        private TopLevel? TopLevel
        {
            get
            {
                return Avalonia.Application.Current!.ApplicationLifetime switch
                {
                    IClassicDesktopStyleApplicationLifetime desktop => TopLevel.GetTopLevel(desktop.MainWindow),
                    ISingleViewApplicationLifetime singleViewPlatform => TopLevel.GetTopLevel(singleViewPlatform.MainView),
                    _ => null
                };
            }
        }

        public async Task<string?> OpenFilePickerAsync(FilePickerOpenOptions options)
        {
            if (TopLevel is null)
                throw new Exception("Could not get the top level window.");

            try
            {
                var files = await TopLevel.StorageProvider.OpenFilePickerAsync(options);

                // If no file is selected (the user cancels), return null
                if (!files.Any())
                    return null;

                return files.First().Path.LocalPath;
            }
            catch (OperationCanceledException)
            {
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }
    }
}