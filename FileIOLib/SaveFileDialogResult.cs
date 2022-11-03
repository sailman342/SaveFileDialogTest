using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileIOLib
{
    public enum SaveFileDialogSuccessCodes {Succes, UserCancelled,  Error,
        InvalidFileName,
        InvalidFolderPath
    }
    public class SaveFileDialogResult
    {
        public SaveFileDialogSuccessCodes SuccessCode { get; internal set; }
        public string Message { get; internal set; }
    }
}
