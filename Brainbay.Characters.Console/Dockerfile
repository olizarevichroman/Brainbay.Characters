FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Brainbay.Characters.Console/Brainbay.Characters.Console.csproj", "Brainbay.Characters.Console/"]
COPY ["Brainbay.Characters.Integrations.RickAndMorty/Brainbay.Characters.Integrations.RickAndMorty.csproj", "Brainbay.Characters.Integrations.RickAndMorty/"]
RUN dotnet restore "Brainbay.Characters.Console/Brainbay.Characters.Console.csproj"
COPY . .
WORKDIR "/src/Brainbay.Characters.Console"
RUN dotnet build "./Brainbay.Characters.Console.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Brainbay.Characters.Console.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Brainbay.Characters.Console.dll"]
