namespace GodelTech.CodeReview.RoslynMetadataExporter.Services
{
    public interface IJsonSerializer
    {
        string Serialize(object data);
    }
}