namespace GodelTech.CodeReview.RoslynMetadataExporter.Services.NuGet
{
    public interface IPackageMetadataProvider
    {
        PackageMetadata Get(string folderPath);
    }
}