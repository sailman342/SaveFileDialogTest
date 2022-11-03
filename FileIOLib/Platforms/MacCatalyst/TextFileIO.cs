using System.Text;

namespace FileIOLib
{
    public partial class TextFileIO
    {
        public static async partial Task<SaveFileDialogResult> SaveStringToFileInPickedFolder(string fileName, string extension, string prompt, string data)
        {
            string path = await FolderPicker.PickFolder(prompt);

            return await SaveStringToFile(path, fileName, extension,  data);
        }

        public static async partial Task<SaveFileDialogResult> SaveStringToFile(string path, string fileName, string extension,  string data)
        {
            string targetFile = Path.Combine(path, fileName);
            if(extension != null && extension != string.Empty) 
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

            return await SaveStringToFile(path, fileName, extension,  sb.ToString());
        }
    }
}
