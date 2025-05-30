﻿FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY /packages /packages
RUN dotnet nuget add source /packages
COPY ["MeadowServer/MeadowServer.csproj", "MeadowServer/"]
RUN dotnet restore "MeadowServer/MeadowServer.csproj"
COPY . .
WORKDIR "/src/MeadowServer"
RUN dotnet build "./MeadowServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MeadowServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MeadowServer.dll"]
