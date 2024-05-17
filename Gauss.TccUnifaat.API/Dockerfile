FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Gauss.TccUnifaat.API/Gauss.TccUnifaat.API.csproj", "Gauss.TccUnifaat.API/"]
COPY ["Gauss.TccUnifaat.Common/Gauss.TccUnifaat.Common.csproj", "Gauss.TccUnifaat.Common/"]
RUN dotnet restore "./Gauss.TccUnifaat.API/Gauss.TccUnifaat.API.csproj"
COPY . .
WORKDIR "/src/Gauss.TccUnifaat.API"
RUN dotnet build "./Gauss.TccUnifaat.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Gauss.TccUnifaat.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gauss.TccUnifaat.API.dll"]