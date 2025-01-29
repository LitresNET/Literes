#!/bin/bash

set -e

# Проверка существования пользователя HangFire
while true; do
    result=$(/opt/mssql-tools/bin/sqlcmd -S ${DB_SERVER} -U ${SA_USER} -P ${SA_PASSWORD} -d ${DB_NAME} -Q "SELECT COUNT(name) FROM sys.database_principals WHERE name = 'HangFire'" -h -1 -W)
    count=$(echo $result | grep -oE '^[0-9]+')

    if [[ -n "$count" && "$count" -eq 1 ]]; then
        >&2 echo "Permission was granted - checking for migrations..."
        break
    else
        >&2 echo "SQL Server is up, but permissions weren't granted yet. Retrying..."
        sleep 5
    fi
done

# Проверка накладывания миграций через наличие таблицы 'Roles'
while true; do
    result=$(/opt/mssql-tools18/bin/sqlcmd -S ${DB_SERVER} -U ${SA_USER} -P ${SA_PASSWORD} -d ${DB_NAME} -Q "IF OBJECT_ID('dbo.Roles', 'U') IS NOT NULL SELECT 1 ELSE SELECT 0" -h -1 -W)
    count=$(echo $result | grep -oE '^[0-9]+')

    if [[ "$count" -eq 1 ]]; then
        >&2 echo "Migrations have been applied - starting server..."
        break
    else
        >&2 echo "Migrations haven't been applied yet. Retrying..."
        sleep 5
    fi
done

# Выполнение команды, переданной в аргументах
exec dotnet Litres.WebAPI.dll