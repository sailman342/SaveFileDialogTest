using FileIOLib;

namespace SaveFileDialogTest;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

    private async void TestButtonClicked(object sender, EventArgs e)
    {
        // prompt and default filename or folder can be useless dependig on target OS 
        // we cannot do anything about it !


#if MACCATALYST || IOS


		string defautlPath = FileSystem.Current.AppDataDirectory.Replace("Library","Documents");
		SaveFileDialog saveFileDialog = new("Sauver une clé","dummy.key","Blabla ligne 1\r\nBlabla ligne 2", $"{defautlPath}");


#elif ANDROID

        string defautlPath = FileSystem.Current.AppDataDirectory;
		SaveFileDialog saveFileDialog = new("Sauver une clé", "dummy.key", "Blabla ligne 1\r\nBlabla ligne 2", $"/storage/emulated/0/Documents"); // {Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}");

#else
		// #elif WINDOWS is not working ? Why ?

		SaveFileDialog saveFileDialog= new("Sauver une clé","dummy.key","Blabla ligne 1\r\nBlabla ligne 2", "c:/");
#endif

        SaveFileDialogResult result = await saveFileDialog.OpenDialogWindowAsync();

        await DisplayAlert("Résultat des courses",$"Code retourné : {result.SuccessCode} \r\nMessage : {result.Message}","OK");
    }
}