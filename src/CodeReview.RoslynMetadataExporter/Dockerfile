#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /app
COPY ["CodeReview.RoslynMetadataExporter/*.csproj", "CodeReview.RoslynMetadataExporter/"]
RUN dotnet restore "CodeReview.RoslynMetadataExporter/CodeReview.RoslynMetadataExporter.csproj" --force

COPY CodeReview.RoslynMetadataExporter/. ./CodeReview.RoslynMetadataExporter/
RUN dotnet publish ./CodeReview.RoslynMetadataExporter/CodeReview.RoslynMetadataExporter.csproj -c Release -o ./CodeReview.RoslynMetadataExporter/out

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS runtime

RUN apt-get update
RUN apt-get --yes install nuget

WORKDIR /app
COPY --from=build /app/CodeReview.RoslynMetadataExporter/out .
ENTRYPOINT ["dotnet", "CodeReview.RoslynMetadataExporter.dll"]
CMD ["--help"]