# Указываем базовый образ SQL Server
FROM mcr.microsoft.com/mssql/server

# Меняем текущего пользователя на пользователя с корневыми правами доступа
USER root

# Устанавливаем переменные среды
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=_BEZ_Par0lya_007_

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