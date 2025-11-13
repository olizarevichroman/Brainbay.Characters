FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Brainbay.Characters.WebApi/Brainbay.Characters.WebApi.csproj", "Brainbay.Characters.WebApi/"]
COPY ["Brainbay.Characters.DataAccess/Brainbay.Characters.DataAccess.csproj", "Brainbay.Characters.DataAccess/"]
RUN dotnet restore "Brainbay.Characters.WebApi/Brainbay.Characters.WebApi.csproj"
COPY . .
WORKDIR "/src/Brainbay.Characters.WebApi"
RUN dotnet build "./Brainbay.Characters.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Brainbay.Characters.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Brainbay.Characters.WebApi.dll"]
