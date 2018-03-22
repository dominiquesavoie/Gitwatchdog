using System.Diagnostics;
using System.Windows.Forms;
using GitWatchdog.Presentation.Services;

namespace GitWatchdog.Services
{
    public class PlatformProvider : IPlatformProvider
    {
        public ProcessStartInfo GetTerminal()
        {
            return new ProcessStartInfo("cmd")
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                ErrorDialog = false,
                UseShellExecute = false
            };
        }

        public string BrowseFolder()
        {
            var dialog = new FolderBrowserDialog
            {
                ShowNewFolderButton = false,
                Description = "Select local git repository",
            };

            var result = dialog.ShowDialog();

            if (result != DialogResult.OK)
            {
                return null;
            }

            return dialog.SelectedPath;
        }
    }
}
