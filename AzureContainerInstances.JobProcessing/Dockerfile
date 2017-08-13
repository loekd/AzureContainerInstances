FROM microsoft/windowsservercore
ARG source
#WORKDIR /app
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT AzureContainerInstances.JobProcessing.exe
