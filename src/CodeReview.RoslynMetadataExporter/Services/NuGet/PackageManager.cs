using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;

namespace GodelTech.CodeReview.RoslynMetadataExporter.Services.NuGet
{
    public class PackageManager : IPackageManager
    {
        private readonly IPackageMetadataProvider _packageMetadataProvider;
        private readonly IFileSystemService _fileSystemService;
        private readonly ILogger<PackageManager> _logger;

        public PackageManager(
            IPackageMetadataProvider packageMetadataProvider,
            IFileSystemService fileSystemService,
            ILogger<PackageManager> logger)
        {
            _packageMetadataProvider = packageMetadataProvider ?? throw new ArgumentNullException(nameof(packageMetadataProvider));
            _fileSystemService = fileSystemService ?? throw new ArgumentNullException(nameof(fileSystemService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public bool Install(string targetFolder, AnalysisRequest request)
        {
            if (string.IsNullOrWhiteSpace(targetFolder))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(targetFolder));
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var parameters = new List<string>
            {
                "install",
                request.PackageId,
                "-OutputDirectory",
                targetFolder
            };

            if (!string.IsNullOrEmpty(request.Framework))
            {
                parameters.Add("-Framework");
                parameters.Add(request.Framework);
            }

            if (string.IsNullOrEmpty(request.PackageVersion))
                return RunNuGet(parameters.ToArray());

            parameters.Add("-Version");
            parameters.Add(request.PackageVersion);

            return RunNuGet(parameters.ToArray());
        }

        public PackageMetadata[] Find(string targetFolder, AnalysisRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(targetFolder))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(targetFolder));

            return
                (from item in _fileSystemService.GetDirectories(targetFolder, "*.*")
                let packageInfo = _packageMetadataProvider.Get(item)
                where packageInfo != null && (request.ScanDependencyPackages || IsMatchingPackage(packageInfo, request))
                select packageInfo)
                .ToArray();
        }

        private static bool IsMatchingPackage(PackageMetadata packageMetadata, AnalysisRequest request)
        {
            if (!packageMetadata.Id.Equals(request.PackageId, StringComparison.OrdinalIgnoreCase))
                return false;

            return string.IsNullOrWhiteSpace(request.PackageVersion) || packageMetadata.Version.Equals(request.PackageVersion, StringComparison.OrdinalIgnoreCase);
        }

        private bool RunNuGet(string[] parameters)
        {
            var builder = new StringBuilder();

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "nuget",
                    Arguments = string.Join(" ", parameters),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                }
            };
            process.OutputDataReceived += (s, e) =>
            {
                builder.AppendLine(e.Data);
            };

            process.Start();

            process.BeginOutputReadLine();
            process.WaitForExit();

            _logger.LogInformation("Exit code: {exitCode}", process.ExitCode);
            _logger.LogInformation(builder.ToString());

            return process.ExitCode == 0;
        }
    }
}
