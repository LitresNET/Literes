FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

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
#WORKDIR /app/tests/Tests.Unit
#RUN dotnet test --verbosity normal

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Litres.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY wait-for-db.sh .
USER root
RUN chmod a+x ./wait-for-db.sh
