namespace GodelTech.CodeReview.RoslynMetadataExporter.Services
{
    public interface IAnalyzerAssemblyPathResolver
    {
        string[] Resolve(string packageRootDir);
    }
}