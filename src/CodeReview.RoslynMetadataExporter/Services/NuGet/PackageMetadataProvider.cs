using System;
using System.IO.Compression;
using System.Linq;

namespace ReviewItEasy.RoslynMetadataExporter.Services.NuGet
{
    public class PackageMetadataProvider : IPackageMetadataProvider
    {
        private readonly IMetadataReader _detailsExtractor;
        private readonly IFileSystemService _fileSystemService;

        public PackageMetadataProvider(
            IMetadataReader detailsExtractor,
            IFileSystemService fileSystemService)
        {
            _detailsExtractor = detailsExtractor ?? throw new ArgumentNullException(nameof(detailsExtractor));
            _fileSystemService = fileSystemService ?? throw new ArgumentNullException(nameof(fileSystemService));
        }

        public PackageMetadata Get(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(folderPath));

            var packageDataFile = _fileSystemService.GetFiles(folderPath, "*.nupkg").FirstOrDefault();
            if (string.IsNullOrWhiteSpace(packageDataFile))
                return null;

            using (var fileStream = _fileSystemService.OpenRead(packageDataFile))
            using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Read))
            {
                var metadataEntry = archive.Entries.FirstOrDefault(x => x.Name.EndsWith(".nuspec", StringComparison.OrdinalIgnoreCase) && !x.FullName.Contains("/"));
                if (metadataEntry == null)
                    return null;

                using (var dataStream = metadataEntry.Open())
                {
                    var package = _detailsExtractor.Read(dataStream);

                    if (package != null)
                        package.FolderPath = folderPath;

                    return package;
                }
            }
        }
    }
}