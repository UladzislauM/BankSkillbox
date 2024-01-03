using MarshalsExceptions;
using Microsoft.Win32;

namespace WpfUI.Services
{
    public class DefaultDialogService : IDialogService
    {
        public string FilePath { get; set; }

        public void OpenFileDialog(string defaultExt, string filter, string title)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.DefaultExt = defaultExt;
                openFileDialog.Filter = filter;
                openFileDialog.Title = title;

                if (openFileDialog.ShowDialog() == true)
                {
                    FilePath = openFileDialog.FileName;
                }
            }
            catch
            {
                throw new JsonException("Something went wrong with open json dialog");
            }
        }

        public void SaveFileDialog(string defaultExt, string filter, string title)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();

                saveFileDialog.DefaultExt = defaultExt;
                saveFileDialog.Filter = filter;
                saveFileDialog.Title = title;

                if (saveFileDialog.ShowDialog() == true)
                {
                    FilePath = saveFileDialog.FileName;
                }
            }
            catch
            {
                throw new JsonException("Something went wrong with save json dialog");
            }
        }

    }
}
