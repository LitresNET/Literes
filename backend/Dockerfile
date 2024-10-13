# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

RUN apt-get update && apt-get install -y \
    curl \
    apt-transport-https \
    gnupg2 \
    unixodbc-dev \
    && curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - \
    && curl https://packages.microsoft.com/config/ubuntu/20.04/prod.list > /etc/apt/sources.list.d/mssql-release.list \
    && apt-get update \
    && ACCEPT_EULA=Y apt-get install -y msodbcsql17 mssql-tools \
    && echo 'export PATH="$PATH:/opt/mssql-tools/bin/sqlcmd"' >> ~/.bashrc \
    && apt-get clean

WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["Litres.WebAPI/Litres.WebAPI.csproj", "Litres.WebAPI/"]
COPY ["Litres.Application/Litres.Application.csproj", "Litres.Application/"]
COPY ["Litres.Domain/Litres.Domain.csproj", "Litres.Domain/"]
COPY ["Litres.Infrastructure/Litres.Infrastructure.csproj", "Litres.Infrastructure/"]

RUN dotnet restore "Litres.WebAPI/Litres.WebAPI.csproj" --verbosity normal

COPY . .

WORKDIR "/src/Litres.WebAPI"
RUN dotnet build "Litres.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Litres.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY wait-for-db.sh .
USER root
RUN chmod a+x ./wait-for-db.sh
ENTRYPOINT ["./wait-for-db.sh"]