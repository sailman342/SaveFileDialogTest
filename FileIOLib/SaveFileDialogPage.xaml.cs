using static FileIOLib.FolderPicker;
using static FileIOLib.TextFileIO;
namespace FileIOLib;

public partial class SaveFileDialogPage : ContentPage
{
    private TaskCompletionSource<SaveFileDialogResult> _tcs = null;

    private string savedFileName;
    private string dataToSave;

    public SaveFileDialogPage(string prompt, string defaultFileNameAndExtension, string defaultFolderPath, string dataToSaveInTextFile, TaskCompletionSource<SaveFileDialogResult> tcs)
    {
        InitializeComponent();

        _tcs = tcs;

        TitleLabel.Text = prompt;
        FileNameEntry.Text = defaultFileNameAndExtension;
        savedFileName = defaultFileNameAndExtension;
        FolderNameLabel.Text = defaultFolderPath;

        dataToSave= dataToSaveInTextFile;

        if (FileNameEntry.Text.Intersect(Path.GetInvalidPathChars()).Count() > 0)
        {
            ReturnInvalidFileName();
        };
        if(!Directory.Exists(defaultFolderPath) || FolderNameLabel.Text.Intersect(Path.GetInvalidPathChars()).Count() > 0)
        {
            ReturnInvalidDirectory();
        }
    }
    private void ReturnUserCancelled()
    {
        SaveFileDialogResult result = new SaveFileDialogResult()
        {
            SuccessCode = SaveFileDialogSuccessCodes.UserCancelled,
            Message = "User cancelled the action"
        };

        _tcs.SetResult(result);
        Navigation.PopAsync();
    }
    private void ReturnInvalidDirectory()
    {
        SaveFileDialogResult result = new SaveFileDialogResult()
        {
            SuccessCode = SaveFileDialogSuccessCodes.InvalidFolderPath,
            Message = "The folder path contains invalid characters or doesn't exist"
        };

        _tcs.SetResult(result);
        Navigation.PopAsync();
    }

    private void ReturnInvalidFileName()
    {
        SaveFileDialogResult result = new SaveFileDialogResult()
        {
            SuccessCode = SaveFileDialogSuccessCodes.InvalidFileName,
            Message = "The filename contains invalid characters according to the operating system naming conventions"
        };

        _tcs.SetResult(result);
        Navigation.PopAsync();
    }

    private void FileNameEntryCompleted(object sender, EventArgs e)
    {
        if (FileNameEntry.Text.Intersect(Path.GetInvalidPathChars()).Count() > 0)
        {
            DisplayAlert("Nom de fichier invalide", $"Le nom de fichier {FileNameEntry.Text} n'est pas acceptable pour {DeviceInfo.Current.Platform}","OK");
            FileNameEntry.Text = savedFileName;
        }
        else
        {
            savedFileName = FileNameEntry.Text;
        }
    }

    private  void CancelButtonClicked(object sender, EventArgs e)
    {
        ReturnUserCancelled();
    }

    private async void SaveButtonClicked(object sender, EventArgs e)
    {
        string fileName = Path.GetFileNameWithoutExtension( FileNameEntry.Text);
        string extension = Path.GetExtension( FileNameEntry.Text );

        SaveFileDialogResult result = await SaveStringToFile(FolderNameLabel.Text, fileName, extension, dataToSave );

        _tcs.SetResult(result);
        await Navigation.PopAsync();
    }

    private async void SelectFolderButtonClicked(object sender, EventArgs e)
    {
        string folderPath = await PickFolder("Choisir un dossier");
        if(folderPath == null || folderPath == string.Empty)
        {
            ReturnUserCancelled();
        }
        else
        {
            FolderNameLabel.Text = folderPath;
        }
    }
}