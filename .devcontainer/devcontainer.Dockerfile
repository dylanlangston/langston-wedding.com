FROM mcr.microsoft.com/devcontainers/base:debian as base

# Add Microsoft package signing key
RUN wget https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
     && dpkg -i packages-microsoft-prod.deb \
     && rm packages-microsoft-prod.deb

# Install General Dependencies
RUN apt-get update && export DEBIAN_FRONTEND=noninteractive \
     && apt-get -y install --no-install-recommends ca-certificates bash curl unzip xz-utils git nodejs npm libsecret-1-0 dotnet-sdk-8.0 azure-functions-core-tools-4

# Install Azure CLI
RUN curl -sL https://aka.ms/InstallAzureCLIDeb | bash

# Clean Image
RUN apt-get clean && rm -rf /var/cache/apt/* && rm -rf /var/lib/apt/lists/* && rm -rf /tmp/*

# Important we change to the vscode user that the devcontainer runs under
USER vscode
WORKDIR /home/vscode