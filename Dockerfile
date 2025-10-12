# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY ListService.csproj ./
RUN dotnet restore "./ListService.csproj"

# Copy source code and build/publish
COPY src/. ./
ARG BUILD_CONFIGURATION=Release
RUN dotnet build "./ListService.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet publish "./ListService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Expose desired ports
EXPOSE 8080
EXPOSE 8081

# Copy published output
COPY --from=build /app/publish ./

# Launch the application
ENTRYPOINT ["dotnet", "ListService.dll"]
