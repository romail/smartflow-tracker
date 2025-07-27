#!/bin/sh
echo "Waiting for Postgres..."

until pg_isready -h db -p 5432 -U postgres; do
  >&2 echo "Postgres is unavailable - sleeping"
  sleep 1
done

echo "Postgres is up - running migrations"
dotnet SmartFlow.Tracker.Migrations.dll
