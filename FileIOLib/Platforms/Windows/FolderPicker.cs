using WindowsFolderPicker = Windows.Storage.Pickers.FolderPicker;

namespace FileIOLib
{
    public partial class FolderPicker
    {
        public async static partial Task<string> PickFolder(string prompt)
        {
            WindowsFolderPicker folderPicker = new()
            {
                //TODO useless and no way to define default folder !
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Downloads,
                CommitButtonText = prompt
            };

            // Get the current window's HWND by passing in the Window object
            var hwnd = ((MauiWinUIWindow)Application.Current.Windows[Application.Current.Windows.Count - 1].Handler.PlatformView).WindowHandle;

            // Associate the HWND with the file picker
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);


            Windows.Storage.StorageFolder folder = await folderPicker.PickSingleFolderAsync();

            if (folder != null)
            {
                return folder.Path;
            }
            else
            {
                return "";
            }
        }
    }
}
