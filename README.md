# SaveFileDialogTest

# **.Net MAUI Folder Picker and Save File Dialog**

Warning : this is not production level code, use it at your own risk .... !

MAUI is a real technological breakthrough for those of us coming from traditional technologies. Running the same code on different Operating Systems and processors, is like a dream for me, more particularly when running it on my phone. But in turn, it seems that  Desktop applications are in some way the poor parent of MAUI. 

One can understand that sandboxing is of prime importance on mobile devices to avoid applications reading other applications data. But on desktop this can become a nightmare for developers as we rely on the Operating System file protection scheme based on user access rights rather than applications rights. And it is clear that a folder picker allowing the desktop application users to decide where to store the application output data is missing in MAUI. 

I attempted to use  [jfversluis/MauiFolderPickerSample: Sample code to demonstrate how to implement a folder picker with .NET MAUI (github.com)](https://github.com/jfversluis/MauiFolderPickerSample) but the Mac platform code is based on a deprecated UIDocumentPickerViewController class constructor

`var picker = new UIDocumentPickerViewController(allowedTypes, UIDocumentPickerMode.Open);`

I based the Windows Folder Picker on Windows.Storage.Pickers.FolderPicker as Gerald did 

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

and I used the Mac Catalyst way for the Mac side

        public partial class FolderPicker
        { 
            public async static partial Task<string> PickFolder(string prompt)
            {
                var customFileType = new FilePickerFileType(
                                new Dictionary<DevicePlatform, IEnumerable<string>>
                                {
                                        { DevicePlatform.iOS, new[] { "public.folder" } }, // UTType values
                                        { DevicePlatform.MacCatalyst, new[] { "public.folder" } }, // UTType values
                                });
    
                PickOptions options = new()
                {
                    PickerTitle = prompt,
                    FileTypes = customFileType,
                };
    
                try
                {
                    var result = await FilePicker.Default.PickAsync(options);
                    if (result != null)
                    {
                        return result.FullPath;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch //(Exception ex)
                {
                    // The user canceled or something went wrong
                    return null;
                }
            }
        }

Simple but it works well.

Then I implemented a small page for choosing the file name and selecting the destination folder and coded something similar to the missing SaveFileDialog function we are all expecting to come soon.

		
		// prompt and default filename or folder can be useless dependig on target OS 
	    // we cannot do anything about it !
	        
		string defautlPath = FileSystem.Current.AppDataDirectory.Replace("Library","Documents");
		
		SaveFileDialog saveFileDialog = new("Sauver une clé","dummy.key",
				"Blabla ligne 1\r\nBlabla ligne 2",
	    		$"{defautlPath}");
	    		
		 SaveFileDialogResult result = await saveFileDialog.OpenDialogWindowAsync();
	
	      await DisplayAlert("Résultat des courses",
	      		$"Code retourné : {result.SuccessCode} \r\nMessage : {result.Message}","OK");
		

As said, nothing really finalised but sufficient to wait for better days when it will hopefully be standard MAUI feature.

