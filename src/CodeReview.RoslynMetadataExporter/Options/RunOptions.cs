using CommandLine;

namespace GodelTech.CodeReview.RoslynMetadataExporter.Options
{
    [Verb("run", true, HelpText = "Runs workflow defined by manifest file.")]
    public class RunOptions
    {
        [Option('p', "package", Required = true, HelpText = "NuGet package identifier")]
        public string PackageId { get; set; }

        [Option('v', "version", Required = false, HelpText = "NuGet package version")]
        public string PackageVersion { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output file path")]
        public string FilePath { get; set; }

        [Option('j', "json", Required = false, HelpText = "Specifies if JSON or YAML output must be used")]
        public bool UseJson { get; set; }
    }
}
