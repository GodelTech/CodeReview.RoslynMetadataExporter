namespace ReviewItEasy.RoslynMetadataExporter.Services
{
    public interface IAnalyzerFileFilter
    {
        bool IsMatch(string relativeFilePath);
    }
}