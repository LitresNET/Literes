# Указываем базовый образ SQL Server
FROM mcr.microsoft.com/mssql/server
EXPOSE 1433

# Меняем текущего пользователя на пользователя с корневыми правами доступа
USER root

RUN apt-get update && apt-get install -y \
    curl \
    apt-transport-https \
    gnupg2 \
    unixodbc-dev \
    && curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - \
    && curl https://packages.microsoft.com/config/ubuntu/20.04/prod.list > /etc/apt/sources.list.d/mssql-release.list \
    && apt-get update \
    && ACCEPT_EULA=Y apt-get install -y msodbcsql17 mssql-tools \
    && echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bashrc \
    && apt-get clean

# Создаем директорию для скриптов инициализации баз данных
RUN mkdir -p /usr/src/app

# Устанавливаем mssql как владельца директории
RUN chown mssql /usr/src/app

# Возвращаем mssql как текущего пользователя
USER mssql
WORKDIR /usr/src/app

# Копируем скрипты инициализации баз данных в контейнер
COPY init-db.sh .
COPY create-databases.sql .

# Меняем текущего пользователя на пользователя с корневыми правами доступа
USER root

# Задаем разрешения на выполнение скрипта инициализации
RUN chmod a+x ./init-db.sh

# Запускаем скрипт инициализации при запуске контейнера
CMD /bin/bash ./init-db.sh
