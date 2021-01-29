using System;
using System.IO;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using GodelTech.CodeReview.RoslynMetadataExporter.Models;
using GodelTech.CodeReview.RoslynMetadataExporter.Options;
using GodelTech.CodeReview.RoslynMetadataExporter.Services;
using GodelTech.CodeReview.RoslynMetadataExporter.Services.Diagnostics;
using GodelTech.CodeReview.RoslynMetadataExporter.Services.NuGet;
using GodelTech.CodeReview.RoslynMetadataExporter.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GodelTech.CodeReview.RoslynMetadataExporter
{
    class Program
    {
        private static int Main(string[] args)
        {
            using var container = CreateServiceProvider();

            var parser = new Parser(x =>
            {
                x.HelpWriter = TextWriter.Null;
            });

            var result = parser.ParseArguments<RunOptions>(args);

            var exitCode = result
                .MapResult(
                    x => ProcessConvertAsync(x, container).GetAwaiter().GetResult(),
                    _ => ProcessErrors(result));

            return exitCode;
        }

        private static async Task<int> ProcessConvertAsync(RunOptions options, ServiceProvider container)
        {
            var request = new AnalysisRequest
            {
                Language = LanguageNames.CSharp,
                PackageId = options.PackageId,
                PackageVersion = options.PackageVersion,
                ScanDependencyPackages = true
            };

            var results = container.GetRequiredService<INuGetPackageProcessor>().Process(request);

            container.GetRequiredService<IResultStorage>().Save(options.FilePath, options.UseJson, results);
            
            return Constants.SuccessExitCode;
        }

        private static int ProcessErrors(ParserResult<RunOptions > result)
        {
            var helpText = HelpText.AutoBuild(result, h =>
            {
                h.AdditionalNewLineAfterOption = false;
                return HelpText.DefaultParsingErrorsHandler(result, h);
            }, e => e);

            Console.WriteLine(helpText);

            return Constants.ErrorExitCode;
        }

        private static ServiceProvider CreateServiceProvider()
        {
            var serviceProvider = new ServiceCollection();

            serviceProvider.AddLogging(x =>
            {
                x.ClearProviders();
                x.AddProvider(new SimplifiedConsoleLoggerProvider());
            });

            serviceProvider.AddSingleton<IFileSystemService, FileSystemService>();

            serviceProvider.AddTransient<IResultStorage, ResultStorage>();
            serviceProvider.AddTransient<IDiagnosticsFactory, DiagnosticsFactory>();
            serviceProvider.AddTransient<IMetadataReader, MetadataReader>();
            serviceProvider.AddTransient<IPackageManager, PackageManager>();
            serviceProvider.AddTransient<IPackageMetadataProvider, PackageMetadataProvider>();
            
            serviceProvider.AddTransient<IPackageMetadataProvider, PackageMetadataProvider>();
            serviceProvider.AddTransient<IAnalyzerAssemblyPathResolver, AnalyzerAssemblyPathResolver>();
            serviceProvider.AddTransient<IAnalyzerFileFilter, AnalyzerFileFilter>();
            
            serviceProvider.AddTransient<IYamlSerializer, YamlSerializer>();
            serviceProvider.AddTransient<IJsonSerializer, JsonSerializer>();

            serviceProvider.AddTransient<INuGetPackageProcessor, NuGetPackageProcessor>();
            serviceProvider.AddTransient<IResultStorage, ResultStorage>();

            return serviceProvider.BuildServiceProvider();
        }
    }
}
