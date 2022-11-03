

namespace FileIOLib
{
    public partial class FolderPicker
    {
        public async static partial Task<string> PickFolder(string prompt)
        {
            string dirStr = $"/storage/emulated/0/Documents"; //    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            await Application.Current.MainPage.DisplayAlert("Un seul dossier acessible", $"Vous ne pouvez travailler qu'avec le dossier \r\n{dirStr}", "OK");
            return dirStr;
        }
    }
}
