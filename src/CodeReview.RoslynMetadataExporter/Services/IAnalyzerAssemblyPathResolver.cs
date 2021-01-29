namespace ReviewItEasy.RoslynMetadataExporter.Services
{
    public interface IAnalyzerAssemblyPathResolver
    {
        string[] Resolve(string packageRootDir);
    }
}