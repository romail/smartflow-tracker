FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SmartFlow.Tracker.Migrations/SmartFlow.Tracker.Migrations.csproj", "SmartFlow.Tracker.Migrations/"]
RUN dotnet restore "SmartFlow.Tracker.Migrations/SmartFlow.Tracker.Migrations.csproj"
COPY . .
WORKDIR "/src/SmartFlow.Tracker.Migrations"
RUN dotnet build "SmartFlow.Tracker.Migrations.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SmartFlow.Tracker.Migrations.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartFlow.Tracker.Migrations.dll"]
