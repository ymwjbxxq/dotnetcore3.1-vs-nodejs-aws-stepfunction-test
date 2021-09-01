# Specify base image
FROM mcr.microsoft.com/dotnet/sdk:3.1-alpine
# Install required OS packages
RUN ["apk", "add", "--no-cache", "bash", "zip"]
# Copy all data from the root folder of the repository 
# to the "build" folder of the image
COPY . ./build
# Make "build" folder working directory
WORKDIR ./build
# Give some permissions to "build.sh" folder
RUN ["chmod", "+x", "./build.sh"]
#Specify "build.sh" as entry point for the image
ENTRYPOINT ./build.sh