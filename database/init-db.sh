#!/bin/bash

/opt/mssql/bin/sqlservr &

# Ожидание запуска SQL Server
sleep 30s

# Выполнение скрипта создания базы данных
/opt/mssql-tools/bin/sqlcmd -S ${DB_SERVER} -U ${SA_USER} -P ${SA_PASSWORD} -v HANGFIRE_PASSWORD=${HANGFIRE_PASSWORD} -i /usr/src/app/create-databases.sql

# Оставляем контейнер активным
while true; do sleep 1d; done