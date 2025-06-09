FROM mcr.microsoft.com/dotnet/sdk:8.0 AS migration
WORKDIR /app

# 1) Install distro protoc
RUN apt-get update \
 && DEBIAN_FRONTEND=noninteractive \
    apt-get install --no-install-recommends --assume-yes \
      protobuf-compiler-grpc

ENV PROTOBUF_PROTOC=/usr/bin/protoc

# 2) Copy only csproj files, restore, and build to catch errors early
COPY ["Litres.Infrastructure/Litres.Infrastructure.csproj", "Litres.Infrastructure/"]
COPY ["Litres.WebAPI/Litres.WebAPI.csproj",       "Litres.WebAPI/"]
COPY ["Litres.Application/Litres.Application.csproj","Litres.Application/"]
COPY ["Litres.Domain/Litres.Domain.csproj",       "Litres.Domain/"]
COPY . .

RUN dotnet restore "Litres.WebAPI/Litres.WebAPI.csproj" -v normal
RUN dotnet build   "Litres.WebAPI/Litres.WebAPI.csproj" \
      --configuration Release --no-restore -v normal

RUN dotnet tool install --global dotnet-ef --version 8.*
ENV PATH="$PATH:/root/.dotnet/tools"

CMD ["dotnet", "ef", "database", "update", "-p", "./Litres.Infrastructure/Litres.Infrastructure.csproj", "-s", "./Litres.WebAPI/Litres.WebAPI.csproj"]
