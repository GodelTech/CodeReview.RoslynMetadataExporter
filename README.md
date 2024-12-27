# CodeReview.RoslynMetadataExporter

Docker Image: https://hub.docker.com/r/godeltech/codereview.roslyn-metadata-exporter

## Description

This tool is used to export metadata of Roslyn analyzers to YAML or JSON format.

## Usage

### How to build the Docker Image

To build the Docker image, run the following command:

```bash
docker build -t codereview.roslyn-metadata-exporter .
```

### How to run the Docker Container

To run the Docker container, use the following command:

```bash
docker run codereview.roslyn-metadata-exporter run -p SonarAnalyzer.CSharp -o result.yaml -j
```

### Commands And Parameters

#### run
Runs workflow defined by manifest file
<pre>
> dotnet CodeReview.RoslynMetadataExporter.dll run -p SonarAnalyzer.CSharp -o result.yaml -j
</pre>

| Agruments | Key | Required | Type   | Description agrument                          |
|-----------|-----|----------|--------|-----------------------------------------------|
| --package | -p  | true     | string | NuGet package identifier                      |
| --version | -v  | false    | string | NuGet package version                         |
| --output  | -o  | true     | string | Output file path                              |
| --json    | -j  | false    | bool   | Specifies if JSON or YAML output must be used |

## License

This project is licensed under the MIT License. See the LICENSE file for more details.