# Запускаем SQL Server
/opt/mssql/bin/sqlservr &

# Ждем, пока SQL Server запустится
sleep 30s

# Выполняем скрипт создания баз данных
/opt/mssql-tools/bin/sqlcmd -S localhost/sqlexpress -U sa -P _BEZ_Par0lya_007_ -i create-databases.sql

# Запускаем бесконечный цикл, чтобы контейнер оставался запущенным
while true; do sleep 1d; done