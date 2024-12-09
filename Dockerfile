FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base

USER root
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ByteWizardApi.csproj", "./"]
RUN dotnet restore "ByteWizardApi.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "ByteWizardApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ByteWizardApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final

# Install WireGuard and needed Tools
RUN apt-get update && apt-get install -y wireguard iproute2 iptables curl

WORKDIR /app
COPY --from=publish /app/publish .

HEALTHCHECK CMD ping -c 1 -W 2 192.168.178.27 || exit 1

ENTRYPOINT ["/bin/bash", "-c", "wg-quick up /etc/wireguard/wg.conf || (echo 'WireGuard failed to start'; exit 1); \
    echo 'nameserver 192.168.178.1' > /etc/resolv.conf && dotnet ByteWizardApi.dll"]