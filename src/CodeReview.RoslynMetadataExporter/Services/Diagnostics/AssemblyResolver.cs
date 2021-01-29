using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace GodelTech.CodeReview.RoslynMetadataExporter.Services.Diagnostics
{
    /// <summary>
    /// Adds additional search directories for assembly resolution
    /// </summary>
    public sealed class AssemblyResolver : IDisposable
    {
        private const string DllExtension = ".dll";

        private readonly string[] _rootSearchPaths;
        private readonly ILogger<AssemblyResolver> _logger;

        /// <summary>
        /// Create a new AssemblyResolver that will search in the given directories (recursively) for dependencies.
        /// </summary>
        public AssemblyResolver(
            ILogger<AssemblyResolver> logger, 
            params string[] rootSearchPaths)
        {
            _rootSearchPaths = rootSearchPaths ?? throw new ArgumentNullException(nameof(rootSearchPaths));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            _logger.LogInformation("Resolving Assembly {assembly}", args.Name);

            var isAssembly = args.Name.EndsWith(DllExtension, StringComparison.OrdinalIgnoreCase);
            var fileName = GetAssemblyFileName(args.Name);

            foreach (var rootSearchPath in _rootSearchPaths)
            {
                foreach (var file in Directory.GetFiles(rootSearchPath, fileName, SearchOption.AllDirectories))
                {
                    var asm = Assembly.LoadFile(file);

                    if (
                        isAssembly && string.Equals(Path.GetFileName(asm.Location), fileName, StringComparison.OrdinalIgnoreCase)
                        ||
                        !isAssembly && string.Equals(args.Name, asm.FullName, StringComparison.OrdinalIgnoreCase)
                    )
                    {
                        _logger.LogInformation("Assembly Located", file);
                        return asm;
                    }

                    _logger.LogInformation("Rejected Assembly", asm.FullName);
                }
            }

            _logger.LogWarning("Failed To Resolve Assembly {assembly}", args.Name);
            return null;
        }

        private static string GetAssemblyFileName(string input)
        {
            if (input.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                return input;

            var assemblyName = new AssemblyName(input);
            return assemblyName.Name + DllExtension;
        }

        #region IDisposable Support

        private bool _disposedValue;

        private void Dispose(bool disposing)
        {
            if (_disposedValue)
                return;

            if (disposing)
                AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;

            _disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}