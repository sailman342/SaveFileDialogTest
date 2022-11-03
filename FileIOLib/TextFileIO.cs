namespace FileIOLib
{
    public partial class TextFileIO
    {
        public enum SuccessCodes { Success, UserCancel, Error }


        // these ones differs from platform to platform implementations are in platform folder ...

        public static partial Task<SaveFileDialogResult> SaveStringToFile(string path, string fileName, string extension,  string data);

        public static partial Task<SaveFileDialogResult> SaveStringArrayToFile(string path, string fileName, string extension,  string[] data);

        public static partial Task<SaveFileDialogResult> SaveStringToFileInPickedFolder(string fileName, string extension, string prompt, string data);


        // these ones are the same for all platforms, implementation built in

        public async static Task<(SuccessCodes, string, string, string[])> PickAndOpenTextFileAsStringArray(string extension, string prompt)
        {
            FileResult result = await PickAFile(extension, prompt);

            if (result != null && result.FileName != "")
            {
                try
                {
                    string inFile = result.FullPath;
                    string[] inFileData = File.ReadAllLines(inFile);
                    return (SuccessCodes.Success, $"Lecture réussie {inFileData.Length}", inFile, inFileData);
                }
                catch (Exception ex)
                {
                    return (SuccessCodes.Error, $"Erreur du File.ReadAllLines \r\n {ex.Message}", "", Array.Empty<string>());
                }
            }
            else
            {
                return (SuccessCodes.UserCancel, $"L'utilisateur a annulé", "", Array.Empty<string>());
            }
        }


        public async static Task<(SuccessCodes, string, string, string)> PickAndOpenTextFileAsString(string extension, string prompt)
        {
            {
                FileResult result = await PickAFile(extension, prompt);

                if (result != null && result.FileName != "")
                {
                    try
                    {
                        string inFile = result.FullPath;
                        string inFileData = File.ReadAllText(inFile);
                        return (SuccessCodes.Success, $"Lecture réussie {inFileData.Length}", inFile, inFileData);
                    }
                    catch (Exception ex)
                    {
                        return (SuccessCodes.Error, $"Erreur du File.ReadAllLines \r\n {ex.Message}", "", String.Empty);
                    }
                }
                else
                {
                    return (SuccessCodes.UserCancel, $"L'utilisateur a annulé", "", String.Empty);
                }
            }
        }

        // local 

        public async static Task<FileResult> PickAFile(string extension, string prompt)
        {

            try
            {

                FilePickerFileType customFileType =
                    new(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                                    { DevicePlatform.iOS, null }, // or general UTType values
                                    { DevicePlatform.Android, null },
                                    { DevicePlatform.WinUI, new[] { $".{extension}" } },
                                    { DevicePlatform.Tizen, new[] { $"*/*" } },
                                    { DevicePlatform.macOS, new[] { $"{extension}" } }, // or general UTType values
                    });

                PickOptions options = new()
                {
                    PickerTitle = $"{prompt}",
                    FileTypes = customFileType,
                };

                FileResult result = await FilePicker.PickAsync(options);

                return result;
            }
            catch // (Exception ex)
            {
                return null;
            }
        }
    }
}
