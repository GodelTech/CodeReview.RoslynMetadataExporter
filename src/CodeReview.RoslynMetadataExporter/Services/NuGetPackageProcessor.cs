using System;
using System.Linq;
using GodelTech.CodeReview.RoslynMetadataExporter.Services.Diagnostics;
using GodelTech.CodeReview.RoslynMetadataExporter.Services.NuGet;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.Extensions.Logging;

namespace GodelTech.CodeReview.RoslynMetadataExporter.Services
{
    public class NuGetPackageProcessor : INuGetPackageProcessor
    {
        private readonly IFileSystemService _fileSystemService;
        private readonly IDiagnosticsFactory _diagnosticsFactory;
        private readonly IPackageManager _packageManager;
        private readonly IAnalyzerAssemblyPathResolver _assemblyPathResolver;
        private readonly ILogger<NuGetPackageProcessor> _logger;

        public NuGetPackageProcessor(
            IFileSystemService fileSystemService,
            IDiagnosticsFactory diagnosticsFactory,
            IPackageManager packageManager,
            IAnalyzerAssemblyPathResolver assemblyPathResolver,
            ILogger<NuGetPackageProcessor> logger)
        {
            _fileSystemService = fileSystemService ?? throw new ArgumentNullException(nameof(fileSystemService));
            _diagnosticsFactory = diagnosticsFactory ?? throw new ArgumentNullException(nameof(diagnosticsFactory));
            _packageManager = packageManager ?? throw new ArgumentNullException(nameof(packageManager));
            _assemblyPathResolver = assemblyPathResolver ?? throw new ArgumentNullException(nameof(assemblyPathResolver));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public PackageDetails[] Process(AnalysisRequest args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            var packagesFolderPath = _fileSystemService.CreateTempDirectory();

            _logger.LogInformation("Installing NuGet package {packageId} {packageVersion}", args.PackageId, args.PackageVersion);

            if (!_packageManager.Install(packagesFolderPath, args))
            {
                _logger.LogError("Failed to install package: {packageId} {packageVersion}", args.PackageId, args.PackageVersion);
                return new[]
                {
                    new PackageDetails
                    {
                        Id = args.PackageId,
                        Version = args.PackageVersion ?? "0.0.0",
                        Diagnostics = new DiagnosticAnalyzer[0]
                    }
                };
            }

            _logger.LogInformation("Package installed");

            var packages = _packageManager.Find(packagesFolderPath, args);

            _logger.LogInformation("Total packages found: {packageCount}", packages.Length);
            _logger.LogInformation("Discovered packages:");

            foreach (var package in packages)
            {
                _logger.LogInformation("Package ID: {packageId}, Version: {versionId}, Folder: {folderPath}", package.Id, package.Version, package.FolderPath);
            }

            return 
                (from package in packages
                 select new PackageDetails
                 {
                    Id = package.Id,
                    Version = package.Version,
                    Description = package.Description,
                    Diagnostics = CreateAnalyzersForPackage(packagesFolderPath, package.FolderPath, args.Language)
                 })
                .ToArray();
        }

        private DiagnosticAnalyzer[] CreateAnalyzersForPackage(string packagesFolder, string packageFolderPath, string language)
        {
            var analyzerFiles = _assemblyPathResolver.Resolve(packageFolderPath);

            return _diagnosticsFactory.Create(
                language, 
                packagesFolder, 
                analyzerFiles);
        }
    }
}
