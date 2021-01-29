using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.Diagnostics;

namespace GodelTech.CodeReview.RoslynMetadataExporter.Services
{
    public class ResultStorage : IResultStorage
    {
        private readonly IFileSystemService _fileSystemService;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IYamlSerializer _yamlSerializer;

        public ResultStorage(
            IFileSystemService fileSystemService,
            IJsonSerializer jsonSerializer,
            IYamlSerializer yamlSerializer)
        {
            _fileSystemService = fileSystemService ?? throw new ArgumentNullException(nameof(fileSystemService));
            _jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
            _yamlSerializer = yamlSerializer ?? throw new ArgumentNullException(nameof(yamlSerializer));
        }

        public void Save(string filePath, bool useJson,  IEnumerable<PackageDetails> packages)
        {
            if (packages == null)
                throw new ArgumentNullException(nameof(packages));

            var result =
                (from package in packages
                 select new
                    {
                        id = package.Id,
                        version = package.Version,
                        description = package.Description,
                        diagnostics = GetDiagnostics(package.Diagnostics)
                    })
                .ToArray();


            var content = useJson ? _jsonSerializer.Serialize(result) : _yamlSerializer.Serialize(result);

            _fileSystemService.WriteAllText(filePath, content);
        }

        private static object[] GetDiagnostics(IEnumerable<DiagnosticAnalyzer> analyzers)
        {
            return
                (from analyzer in analyzers
                    from diagnostic in analyzer.SupportedDiagnostics
                    orderby diagnostic.Id
                    select new
                    {
                        diagnostic.Id,
                        Title = diagnostic.Title.ToString(),
                        DefaultSeverity = diagnostic.DefaultSeverity.ToString(),
                        diagnostic.IsEnabledByDefault,
                        diagnostic.Category,
                        Description = diagnostic.Description.ToString(),
                        diagnostic.HelpLinkUri,
                        MessageFormat = diagnostic.MessageFormat.ToString(),
                        CustomTags = diagnostic.CustomTags.ToArray(),
                    })
                .Cast<object>()
                .ToArray();
        }
    }
}