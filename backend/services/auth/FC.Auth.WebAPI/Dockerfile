# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/auth/FC.Auth.WebAPI/FC.Auth.WebAPI.csproj", "services/auth/FC.Auth.WebAPI/"]
COPY ["buildingblocks/FC.BuildingBlocks.Application/FC.BuildingBlocks.Application.csproj", "buildingblocks/FC.BuildingBlocks.Application/"]
COPY ["buildingblocks/FC.BuildingBlocks.WebAPI/FC.BuildingBlocks.WebAPI.csproj", "buildingblocks/FC.BuildingBlocks.WebAPI/"]
COPY ["services/auth/FC.Auth.Application/FC.Auth.Application.csproj", "services/auth/FC.Auth.Application/"]
COPY ["services/auth/FC.Auth.Domain/FC.Auth.Domain.csproj", "services/auth/FC.Auth.Domain/"]
COPY ["buildingblocks/FC.BuildingBlocks.Domain/FC.BuildingBlocks.Domain.csproj", "buildingblocks/FC.BuildingBlocks.Domain/"]
COPY ["services/auth/FC.Auth.Infrastructure/FC.Auth.Infrastructure.csproj", "services/auth/FC.Auth.Infrastructure/"]
RUN dotnet restore "./services/auth/FC.Auth.WebAPI/FC.Auth.WebAPI.csproj"
COPY . .
WORKDIR "/src/services/auth/FC.Auth.WebAPI"
RUN dotnet build "./FC.Auth.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FC.Auth.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FC.Auth.WebAPI.dll"]