namespace WpfUI.Services
{
    public interface IDialogService
    {
        string FilePath { get; set; }
        void OpenFileDialog(string defaultExt, string filter, string title);
        void SaveFileDialog(string defaultExt, string filter, string title);
    }
}
