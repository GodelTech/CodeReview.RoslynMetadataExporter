namespace GodelTech.CodeReview.RoslynMetadataExporter.Services
{
    public interface IAnalyzerFileFilter
    {
        bool IsMatch(string relativeFilePath);
    }
}