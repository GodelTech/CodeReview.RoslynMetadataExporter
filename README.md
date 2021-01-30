# Introduction 

How to build image 

docker build -t godeltech/codereview.roslyn-metadata-exporter:0.0.1 -f src/CodeReview.RoslynMetadataExporter/Dockerfile ./src
docker image tag godeltech/codereview.roslyn-metadata-exporter:0.0.1 godeltech/codereview.roslyn-metadata-exporter:latest
docker push godeltech/codereview.roslyn-metadata-exporter:latest
docker push godeltech/codereview.roslyn-metadata-exporter:0.0.1

Run:

docker run -v "/d/temp:/result"   --rm godeltech/codereview.roslyn-metadata-exporter run -p SonarAnalyzer.CSharp -o /result/result.yaml -j

Debug:

docker run -v "/d/temp:/result" -it --rm  --entrypoint /bin/bash  godeltech/codereview.roslyn-metadata-exporter