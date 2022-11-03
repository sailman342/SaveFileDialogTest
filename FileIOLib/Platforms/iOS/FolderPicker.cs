using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileIOLib
{
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
}
