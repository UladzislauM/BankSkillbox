namespace Bank
{
    public interface IFileService
    {
        List<E> Open<E>(string filename);
        void Save<E>(string filename, List<E> phonesList);
    }
}
