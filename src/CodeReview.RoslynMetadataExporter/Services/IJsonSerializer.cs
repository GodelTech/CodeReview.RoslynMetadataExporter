namespace ReviewItEasy.RoslynMetadataExporter.Services
{
    public interface IJsonSerializer
    {
        string Serialize(object data);
    }
}