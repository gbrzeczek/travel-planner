FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/TravelPlanner.Api/TravelPlanner.Api.csproj", "src/TravelPlanner.Api/"]
RUN dotnet restore "src/TravelPlanner.Api/TravelPlanner.Api.csproj"
COPY . .
WORKDIR "/src/src/TravelPlanner.Api"
RUN dotnet build "TravelPlanner.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TravelPlanner.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TravelPlanner.Api.dll"]
