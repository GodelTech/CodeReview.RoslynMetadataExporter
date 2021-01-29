using System;
using System.IO;
using System.Linq;

namespace ReviewItEasy.RoslynMetadataExporter.Services
{
    public class AnalyzerAssemblyPathResolver : IAnalyzerAssemblyPathResolver
    {
        private readonly IFileSystemService _fileSystemService;
        private readonly IAnalyzerFileFilter _analyzerFileFilter;

        public AnalyzerAssemblyPathResolver(
            IFileSystemService fileSystemService, 
            IAnalyzerFileFilter analyzerFileFilter)
        {
            _fileSystemService = fileSystemService ?? throw new ArgumentNullException(nameof(fileSystemService));
            _analyzerFileFilter = analyzerFileFilter ?? throw new ArgumentNullException(nameof(analyzerFileFilter));
        }

        public string[] Resolve(string packageRootDir)
        {
            if (string.IsNullOrWhiteSpace(packageRootDir))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(packageRootDir));

            var allFiles = _fileSystemService.GetFiles(packageRootDir, "*", SearchOption.AllDirectories);

            return
                (from file in allFiles
                    let relativePath = file.Substring(packageRootDir.Length + 1)
                    where _analyzerFileFilter.IsMatch(relativePath)
                 select file)
                .ToArray();
        }
    }
}