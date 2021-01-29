using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ReviewItEasy.RoslynMetadataExporter.Services
{
    public class AnalyzerFileFilter : IAnalyzerFileFilter
    {
        // Default patterns 
        //   "analyzers\\dotnet\\cs",
        //   "analyzers\\dotnet",
        //   "analyzers"
        // But some packages use invalid conventions:
        //   "analyzers\\net451"
        private static readonly Regex[] Patterns = 
        {
            new Regex(@"analyzers\\[^\\]+\\cs\\[^\\]+.dll", RegexOptions.IgnoreCase | RegexOptions.Singleline),
            new Regex(@"analyzers\\[^\\]+\\[^\\]+.dll", RegexOptions.IgnoreCase | RegexOptions.Singleline),
            new Regex(@"analyzers\\[^\\]+.dll", RegexOptions.IgnoreCase | RegexOptions.Singleline),
        };

        public bool IsMatch(string relativeFilePath)
        {
            if (string.IsNullOrWhiteSpace(relativeFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(relativeFilePath));

            relativeFilePath = relativeFilePath.Replace("/", "\\");
            
            return Patterns.Any(x => x.IsMatch(relativeFilePath));
        }
    }
}