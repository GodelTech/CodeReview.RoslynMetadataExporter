using System.Collections.Generic;

namespace GodelTech.CodeReview.RoslynMetadataExporter.Services
{
    public interface IResultStorage
    {
        void Save(string filePath, bool useJson, IEnumerable<PackageDetails> packages);
    }
}