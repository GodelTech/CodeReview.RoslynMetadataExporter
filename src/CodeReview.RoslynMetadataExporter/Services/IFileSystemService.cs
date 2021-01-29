using System.IO;

namespace ReviewItEasy.RoslynMetadataExporter.Services
{
    public interface IFileSystemService
    {
        string CreateTempDirectory();
        string[] GetFiles(string folderPath, string pattern);
        string[] GetFiles(string folderPath, string pattern, SearchOption option);
        string[] GetDirectories(string folderPath, string pattern);
        void WriteAllText(string filePath, string content);
        Stream OpenRead(string path);
    }
}