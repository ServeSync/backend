FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Presentation/ServeSync.API/ServeSync.API.csproj", "Presentation/ServeSync.API/"]
COPY ["Core/ServeSync.Application/ServeSync.Application.csproj", "Core/ServeSync.Application/"]
COPY ["Core/ServeSync.Domain/ServeSync.Domain.csproj", "Core/ServeSync.Domain/"]
COPY ["Infrastructure/ServeSync.Infrastructure.EfCore/ServeSync.Infrastructure.EfCore.csproj", "Infrastructure/ServeSync.Infrastructure.EfCore/"]
COPY ["Infrastructure/ServeSync.Infrastructure.Identity/ServeSync.Infrastructure.Identity.csproj", "Infrastructure/ServeSync.Infrastructure.Identity/"]
COPY ["Infrastructure/ServeSync.Infrastructure/ServeSync.Infrastructure.csproj", "Infrastructure/ServeSync.Infrastructure/"]
COPY ["Infrastructure/ServeSync.Infrastructure.MongoDb/ServeSync.Infrastructure.MongoDb.csproj", "Infrastructure/ServeSync.Infrastructure.MongoDb/"]
RUN dotnet restore "Presentation/ServeSync.API/ServeSync.API.csproj"
COPY . .
WORKDIR "/src/Presentation/ServeSync.API"
RUN dotnet build "ServeSync.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ServeSync.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ServeSync.API.dll"]
