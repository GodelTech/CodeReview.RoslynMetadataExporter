using System.IO;

namespace ReviewItEasy.RoslynMetadataExporter.Services.NuGet
{
    public interface IMetadataReader
    {
        PackageMetadata Read(Stream dataStream);
    }
}