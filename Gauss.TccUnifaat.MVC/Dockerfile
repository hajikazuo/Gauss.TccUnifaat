FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
EXPOSE 80
WORKDIR /App

WORKDIR /src
COPY ["Gauss.TccUnifaat.MVC/Gauss.TccUnifaat.MVC.csproj", "Gauss.TccUnifaat.MVC/"]
COPY ["Gauss.TccUnifaat.Common/Gauss.TccUnifaat.Common.csproj", "Gauss.TccUnifaat.Common/"]
RUN dotnet restore -f "Gauss.TccUnifaat.MVC/Gauss.TccUnifaat.MVC.csproj"

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore 

# Copy everything else and build
COPY . ./ ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "Gauss.TccUnifaat.MVC.dll"]