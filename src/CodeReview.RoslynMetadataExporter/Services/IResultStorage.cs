using System.Collections.Generic;

namespace ReviewItEasy.RoslynMetadataExporter.Services
{
    public interface IResultStorage
    {
        void Save(string filePath, bool useJson, IEnumerable<PackageDetails> packages);
    }
}