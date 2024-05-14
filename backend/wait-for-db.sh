#!/bin/bash

set -e

cmd="$1"
shift
cmd="$@"

# Ждем пока бд запустится и будет готова принимать запросы
>&2 echo "SQL Server is unavailable - sleeping..."
sleep 20s

>&2 echo "SQL Server is up - executing command"
exec $cmd
