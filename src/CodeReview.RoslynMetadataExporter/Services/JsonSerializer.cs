using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GodelTech.CodeReview.RoslynMetadataExporter.Services
{
    public class JsonSerializer : IJsonSerializer
    {
        private static readonly JsonSerializerSettings Settings = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public string Serialize(object data)
        {
            if (data == null) 
                throw new ArgumentNullException(nameof(data));
            
            return JsonConvert.SerializeObject(data, Settings);
        }
    }
}