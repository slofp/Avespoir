FROM mcr.microsoft.com/dotnet/sdk:3.1 as Build

COPY . /Avespoir

WORKDIR /Avespoir

RUN dotnet restore; \
	dotnet build -c Release; \
	dotnet publish ./Avespoir -o ./Builds/bin/ -c Release --self-contained true -r linux-x64 -p:PublishReadyToRun=true -p:PublishSingleFile=false -p:PublishTrimmed=true -p:PublishReadyToRunShowWarnings=true

RUN mv docker-entrypoint.sh ./Builds/bin


FROM ubuntu:focal as Bot

COPY --from=Build /Avespoir/Builds/bin /Avespoir

RUN apt update; \
	apt install -y libicu66 libcurl4

WORKDIR /Avespoir

CMD ["/bin/bash", "-c", "./docker-entrypoint.sh"]
