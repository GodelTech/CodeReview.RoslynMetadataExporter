# Introduction 

How to build image 

docker build -t diamonddragon/roslyn-metadata-exporter -f src/ReviewItEasy.RoslynMetadataExporter/Dockerfile ./src

Run:

docker run -v "/d/temp:/result"   --rm diamonddragon/roslyn-metadata-exporter run -p SonarAnalyzer.CSharp -o /result/result.yaml -j

Debug:

docker run -v "/d/temp:/result" -it --rm  --entrypoint /bin/bash  diamonddragon/roslyn-metadata-exporter