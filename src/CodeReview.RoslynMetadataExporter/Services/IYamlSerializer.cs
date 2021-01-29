namespace ReviewItEasy.RoslynMetadataExporter.Services
{
    public interface IYamlSerializer
    {
        string Serialize(object data);
    }
}