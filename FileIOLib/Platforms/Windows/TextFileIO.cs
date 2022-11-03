using System.Text;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using WindowsFileSavePicker = Windows.Storage.Pickers.FileSavePicker;

namespace FileIOLib
{
    public partial class TextFileIO
    {
        public static async partial Task<SaveFileDialogResult> SaveStringToFileInPickedFolder(string fileName, string extension, string prompt, string data)
        {
            WindowsFileSavePicker savePicker = new()
            {
                SuggestedStartLocation = PickerLocationId.Downloads
            };
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add($"{prompt}", new List<string>() { $".{extension.Replace(".","")}" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = $"{fileName}";

            // Get the current window's HWND by passing in the Window object
            var hwnd = ((MauiWinUIWindow)Application.Current.Windows[Application.Current.Windows.Count - 1].Handler.PlatformView).WindowHandle;

            // Associate the HWND with the file picker
            WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hwnd);

            try
            {
                StorageFile file = await savePicker.PickSaveFileAsync();
                if (file != null)
                {
                    // Prevent updates to the remote version of the file until we finish making changes and call CompleteUpdatesAsync.
                    CachedFileManager.DeferUpdates(file);
                    // write to file
                    await Windows.Storage.FileIO.WriteTextAsync(file, data);
                    // Let Windows know that we're finished changing the file so the other app can update the remote version of the file.
                    // Completing updates may require Windows to ask for user input.
                    FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                    if (status == FileUpdateStatus.Complete)
                    {
                        SaveFileDialogResult result = new SaveFileDialogResult()
                        {
                            SuccessCode = SaveFileDialogSuccessCodes.Succes,
                            Message = "Le fichier " + file.Name + " a été sauvé"
                        };
                        return (result);
                    }
                    else
                    {
                        SaveFileDialogResult result = new SaveFileDialogResult()
                        {
                            SuccessCode = SaveFileDialogSuccessCodes.Error,
                            Message = "Le fichier " + file.Name + $" n'a pu être sauvé {status.ToString()}"
                        };
                        return (result);
                    }
                }
                else
                {
                    SaveFileDialogResult result = new SaveFileDialogResult()
                    {
                        SuccessCode = SaveFileDialogSuccessCodes.UserCancelled,
                        Message = $"L'utilisateur a annulé"
                    };
                    return (result);
                }

            }
            catch (Exception ex)
            {
                SaveFileDialogResult result = new SaveFileDialogResult()
                {
                    SuccessCode = SaveFileDialogSuccessCodes.Error,
                    Message = $"Erreur interne\r\n {ex.Message}"
                };
                return (result);
            }
        }

        public static async partial Task<SaveFileDialogResult> SaveStringToFile(string path, string fileName, string extension,  string data)
        {

            // We don't have a storage file use streams and write async !

            string targetFile = Path.Combine(path, fileName);
            if (extension != null && extension != string.Empty)
            {
                targetFile = Path.Combine(path, fileName + "." + extension);
            }

            using FileStream outputStream = System.IO.File.OpenWrite(targetFile);
            using StreamWriter streamWriter = new StreamWriter(outputStream);

            try
            {
                await streamWriter.WriteAsync(data);
                SaveFileDialogResult result = new SaveFileDialogResult()
                {
                    SuccessCode = SaveFileDialogSuccessCodes.Succes,
                    Message = "Le fichier " + targetFile + " a été sauvé"
                };
                return (result);
            }
            catch (Exception ex)
            {
                SaveFileDialogResult result = new SaveFileDialogResult()
                {
                    SuccessCode = SaveFileDialogSuccessCodes.Error,
                    Message = $"Erreur interne\r\n {ex.Message}"
                };
                return (result);
            }
        }
        public static async partial Task<SaveFileDialogResult> SaveStringArrayToFile(string path, string fileName, string extension,  string[] data)
        {
            StringBuilder sb = new();

            foreach (string line in data)
            {
                sb.Append(line + "\r\n");
            }

            return await SaveStringToFile(path, fileName, extension, sb.ToString());
        }
    }
}
