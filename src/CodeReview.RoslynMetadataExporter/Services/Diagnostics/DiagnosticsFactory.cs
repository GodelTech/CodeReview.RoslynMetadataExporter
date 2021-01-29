using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.Extensions.Logging;

namespace ReviewItEasy.RoslynMetadataExporter.Services.Diagnostics
{
    public class DiagnosticsFactory : IDiagnosticsFactory
    {
        private readonly ILogger<DiagnosticsFactory> _logger;
        private readonly ILoggerFactory _loggerFactory;

        public DiagnosticsFactory(
            ILogger<DiagnosticsFactory> logger,
            ILoggerFactory loggerFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        /// <summary>
        /// Loads all of the given assemblies and instantiates Roslyn diagnostic objects - i.e. existing types deriving from
        /// <see cref="DiagnosticAnalyzer"/>. Non-assembly analyzerAssemblies will be ignored.
        /// </summary>
        /// <returns>Enumerable with instances of DiagnosticAnalyzer from discovered assemblies</returns>
        public DiagnosticAnalyzer[] Create(string language, string packageFolder, string[] analyzerAssemblies)
        {
            using (new AssemblyResolver(_loggerFactory.CreateLogger<AssemblyResolver>(), packageFolder))
            {
                return
                    (from assemblyPath in analyzerAssemblies.Where(filePath => filePath.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                    from diagnostic in InstantiateDiagnosticsFromAssembly(assemblyPath, language)
                    select diagnostic)
                    .ToArray();
            }
        }

        private IEnumerable<DiagnosticAnalyzer> InstantiateDiagnosticsFromAssembly(string assemblyPath, string language)
        {
            var analyzerAssembly = Assembly.LoadFrom(assemblyPath);

            try
            {
                var analyzers = InstantiateDiagnosticAnalyzers(analyzerAssembly, language);

                _logger.LogInformation("Total analyzers found {count} in assembly {assembly}", analyzers.Count, analyzerAssembly.ToString());

                return analyzers;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed load types from assembly {fullName}", analyzerAssembly.FullName);
            }

            return Enumerable.Empty<DiagnosticAnalyzer>();
        }

        private IReadOnlyCollection<DiagnosticAnalyzer> InstantiateDiagnosticAnalyzers(Assembly analyserAssembly, string language)
        {
            var analyzers = new List<DiagnosticAnalyzer>();

            // IMPORTANT: Assembly resolve is used here to load dependencies
            foreach (var type in analyserAssembly.GetTypes())
            {
                if (!type.IsAbstract &&
                    type.IsSubclassOf(typeof(DiagnosticAnalyzer)) &&
                    DiagnosticMatchesLanguage(type, language))
                {

                    try
                    {
                        var analyzer = (DiagnosticAnalyzer)Activator.CreateInstance(type);
                        analyzers.Add(analyzer);
                        _logger.LogDebug("Scanner Analyzer Loaded {analyzer}", analyzer.ToString());
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to create analyzer of type {type}", type);
                    }
                }
            }

            return analyzers;
        }

        private static bool DiagnosticMatchesLanguage(MemberInfo type, string language)
        {
            var analyzerAttribute = type.GetCustomAttribute<DiagnosticAnalyzerAttribute>();

            return analyzerAttribute != null && analyzerAttribute.Languages.Any(l => string.Equals(l, language, StringComparison.OrdinalIgnoreCase));
        }
    }
}