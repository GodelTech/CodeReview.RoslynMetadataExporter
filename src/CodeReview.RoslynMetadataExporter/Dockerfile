#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app
COPY ["ReviewItEasy.RoslynMetadataExporter/*.csproj", "ReviewItEasy.RoslynMetadataExporter/"]
RUN dotnet restore "ReviewItEasy.RoslynMetadataExporter/ReviewItEasy.RoslynMetadataExporter.csproj" --force

COPY ReviewItEasy.RoslynMetadataExporter/. ./ReviewItEasy.RoslynMetadataExporter/
RUN dotnet publish ./ReviewItEasy.RoslynMetadataExporter/ReviewItEasy.RoslynMetadataExporter.csproj -c Release -o ./ReviewItEasy.RoslynMetadataExporter/out

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS runtime

RUN apt-get update
RUN apt-get --yes install nuget

WORKDIR /app
COPY --from=build /app/ReviewItEasy.RoslynMetadataExporter/out .
ENTRYPOINT ["dotnet", "ReviewItEasy.RoslynMetadataExporter.dll"]
CMD ["--help"]