#!/bin/bash
dotnet publish ./StopRunningStepFunctionExecution.csproj --configuration "Release" --framework "netcoreapp3.1" /p:PublishReadyToRun=true /p:GenerateRuntimeConfigurationFiles=true --runtime linux-x64 --self-contained false
# Create a new directory where we will put deployment package
mkdir -p /package
# # Go to bin folder with built project
cd ./bin/Release/netcoreapp3.1/publish
# # Archive deployment package and store it to the specified folder
zip -9 -r /package/deployment-archive.zip . || exit 1
# # # Create the output directory in the mounted volume
mkdir -p /volume/package
# # # Copy the deployment package to the folder in the mounted volume
cd /package
cp -a deployment-archive.zip /volume/package || exit 1