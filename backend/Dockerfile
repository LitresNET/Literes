# Используем официальный образ .Net8 как базовый для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ENV DOTNET_EnableWriteXorExecute=0

# # Устанавливаем инструменты EF Core
# ENV PATH $PATH:/root/.dotnet/tools
# RUN dotnet tool install --global dotnet-ef --version 8.*

# Устанавливаем рабочую директорию в контейнере
WORKDIR /app

# Копируем проекты в рабочую директорию как отдельные слои
COPY *.sln .
COPY src/Litres.Data/*csproj ./src/Litres.Data/
COPY src/Litres.Main/*csproj ./src/Litres.Main/
COPY tests/Tests.Unit/*.csproj ./tests/Tests.Unit/
COPY tests/Tests.Integration/*.csproj ./tests/Tests.Integration/

COPY . .

# Устанавливаем зависимости проекта
RUN dotnet restore --verbosity normal

# Копируем оставшиеся файлы проекта и собираем приложение
RUN dotnet build -c release --verbosity normal --no-restore

# # Ожидаем, пока база данных запустится
# CMD ["wait-for-it", "database:1433", "--", "dotnet", "ef", "database", "update", "-s", "Litres.Main/Litres.Main.csproj", "-p", "Litres.Data/Litres.Data.csproj"]

# Тестируем приложение
# WORKDIR /app/tests/Tests.Unit
# RUN dotnet test --verbosity normal

# Публикуем приложение
WORKDIR /app/src/Litres.Main
RUN dotnet publish -c release -o /app

# Создаем образ ASP.NET для запуска приложения
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./

# Запускаем приложение
ENTRYPOINT ["dotnet", "Litres.Main.dll"]