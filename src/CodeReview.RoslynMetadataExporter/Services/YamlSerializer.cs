using System;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace GodelTech.CodeReview.RoslynMetadataExporter.Services
{
    public class YamlSerializer : IYamlSerializer
    {
        public string Serialize(object data)
        {
            if (data == null) 
                throw new ArgumentNullException(nameof(data));

            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            return serializer.Serialize(data);
        }
    }
}
