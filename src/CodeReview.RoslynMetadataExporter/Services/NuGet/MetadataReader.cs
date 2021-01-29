using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace GodelTech.CodeReview.RoslynMetadataExporter.Services.NuGet
{
    public class MetadataReader : IMetadataReader
    {
        public PackageMetadata Read(Stream dataStream)
        {
            if (dataStream == null)
                throw new ArgumentNullException(nameof(dataStream));

            var document = XDocument.Load(dataStream);

            var metadata = GetByLocalName(document.Root, "metadata");
            if (metadata == null)
                return null;

            var id = GetByLocalName(metadata, "id")?.Value;
            var version = GetByLocalName(metadata, "version")?.Value;
            var description = GetByLocalName(metadata, "description")?.Value;

            if (string.IsNullOrEmpty(id))
                return null;

            return new PackageMetadata
            {
                Id = id,
                Version = version,
                Description = description
            };
        }

        private static XElement GetByLocalName(XContainer package, string name)
        {
            return package.Elements().FirstOrDefault(x => x.Name.LocalName == name);
        }
    }
}