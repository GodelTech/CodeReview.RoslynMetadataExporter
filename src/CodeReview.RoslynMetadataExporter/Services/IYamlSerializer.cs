namespace GodelTech.CodeReview.RoslynMetadataExporter.Services
{
    public interface IYamlSerializer
    {
        string Serialize(object data);
    }
}