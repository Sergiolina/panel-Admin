# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["panel-Admin.csproj", "./"]

RUN dotnet restore "./panel-Admin.csproj"

COPY . .

RUN dotnet publish "./panel-Admin.csproj" -c Release -o /app/publish /p:UseAppHost=false


# Etapa de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:10000

EXPOSE 10000

ENTRYPOINT ["dotnet", "panel-Admin.dll"]
