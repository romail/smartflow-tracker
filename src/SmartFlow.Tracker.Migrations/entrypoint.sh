#!/bin/sh
set -e

echo "Waiting for Postgres..."

# Извлечь хост и порт из CONNECTION_STRING
HOST=$(echo "$CONNECTION_STRING" | sed -n 's/.*Host=\([^;]*\).*/\1/p')
PORT=$(echo "$CONNECTION_STRING" | sed -n 's/.*Port=\([^;]*\).*/\1/p')

until pg_isready -h "$HOST" -p "$PORT"; do
  >&2 echo "Postgres is unavailable - sleeping"
  sleep 1
done

echo "Postgres is up - running migrations..."
dotnet SmartFlow.Tracker.Migrations.dll "$CONNECTION_STRING"
