using System.IO;

namespace GodelTech.CodeReview.RoslynMetadataExporter.Services.NuGet
{
    public interface IMetadataReader
    {
        PackageMetadata Read(Stream dataStream);
    }
}