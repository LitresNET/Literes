# Base image for migrations
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS migration

WORKDIR /app

COPY ["./Litres.Infrastructure/Litres.Infrastructure.csproj", "./Litres.Infrastructure/"]
COPY ["./Litres.WebAPI/Litres.WebAPI.csproj", "./Litres.WebAPI/"]
COPY ["./Litres.Application/Litres.Application.csproj", "./Litres.Application/"]
COPY ["./Litres.Domain/Litres.Domain.csproj", "./Litres.Domain/"]

RUN dotnet restore "./Litres.WebAPI/Litres.WebAPI.csproj" -v normal

COPY . .

RUN dotnet tool install --global dotnet-ef --version 8.*
ENV PATH="$PATH:/root/.dotnet/tools"
CMD ["dotnet", "ef", "database", "update", "-p", "./Litres.Infrastructure/Litres.Infrastructure.csproj", "-s", "./Litres.WebAPI/Litres.WebAPI.csproj"]