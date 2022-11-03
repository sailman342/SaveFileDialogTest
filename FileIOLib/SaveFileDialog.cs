using System.Text;

namespace FileIOLib
{
    public class SaveFileDialog
    {
        private string _filename;
        private string _path;
        private string _text;
        private string _windowTitle;

        private TaskCompletionSource<SaveFileDialogResult> _tcs = null;

        public SaveFileDialog(string windowTitle, string defaultFileName, string outputText, string defaultPath = "")
        {
            _path = defaultPath;
            _filename = defaultFileName;
            _text = outputText;
            _windowTitle = windowTitle;
        }

        public SaveFileDialog(string windowTitle, string defaultFileName, string[] outputTextArray, string defaultPath = "")
        {
            _path = defaultPath;
            _filename = defaultFileName;
            StringBuilder s = new();
            foreach (string str in outputTextArray)
            {
                s.Append(str);
            }
            _text = s.ToString();
            _windowTitle = windowTitle;
        }

        public async Task<SaveFileDialogResult> OpenDialogWindowAsync()
        {

            SaveFileDialogResult result = new();

            // Users may prefer to use the SaveFileDialogPage anyway rather then the Windows savepicker, delete the next lines in that case ...

#if WINDOWS
            string extension = Path.GetExtension(_filename);
            string fileName = Path.GetFileNameWithoutExtension(_filename);
            result = await TextFileIO.SaveStringToFileInPickedFolder(fileName, extension, _windowTitle, _text);
#else

            _tcs = new TaskCompletionSource<SaveFileDialogResult>();


            SaveFileDialogPage thePage = new(_windowTitle, _filename, _path, _text, _tcs);
            Page mainPage = Application.Current.MainPage;
            await mainPage.Navigation.PushAsync(thePage);

            result = await _tcs.Task;



#endif
            return result;
        }
    }
}
