using System;
using System.IO;

namespace ReviewItEasy.RoslynMetadataExporter.Services
{
    public class FileSystemService : IFileSystemService
    {
        public string CreateTempDirectory()
        {
            var newPath = Directory.GetCurrentDirectory();

            newPath = Path.Combine(newPath, "temp", Guid.NewGuid().ToString());
            Directory.CreateDirectory(newPath);

            return newPath;
        }

        public string[] GetFiles(string folderPath, string pattern)
        {
            return Directory.GetFiles(folderPath, pattern);
        }

        public string[] GetFiles(string folderPath, string pattern, SearchOption option)
        {
            return Directory.GetFiles(folderPath, pattern, option);
        }

        public string[] GetDirectories(string folderPath, string pattern)
        {
            return Directory.GetDirectories(folderPath, pattern);
        }

        public void WriteAllText(string filePath, string content)
        {
            File.WriteAllText(filePath, content);
        }

        public Stream OpenRead(string path)
        {
            return File.OpenRead(path);
        }
    }
}