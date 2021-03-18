# Introduction 

#### run
Runs workflow defined by manifest file
<pre>
> dotnet CodeReview.RoslynMetadataExporter.dll run -p SonarAnalyzer.CSharp -o result.yaml -j
</pre>
| Agruments     | Key       | Required   | Type      | Description agrument      |
| ------------- | --------- | ---------- | --------- | ------------------------- |
| --package     | -p        | true       | string    | NuGet package identifier  |
| --version     | -v        | false      | string    | NuGet package version     |
| --output      | -o        | true       | string    | Output file path          |
| --json        | -j        | false      | bool      | Specifies if JSON or YAML output must be used |