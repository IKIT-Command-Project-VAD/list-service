FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files for restore
COPY ["src/ShoppingList.List.Web/ShoppingList.List.Web.csproj", "src/ShoppingList.List.Web/"]
COPY ["src/ShoppingList.List.Core/ShoppingList.List.Core.csproj", "src/ShoppingList.List.Core/"]
COPY ["src/ShoppingList.List.Infrastructure/ShoppingList.List.Infrastructure.csproj", "src/ShoppingList.List.Infrastructure/"]
COPY ["src/ShoppingList.List.UseCases/ShoppingList.List.UseCases.csproj", "src/ShoppingList.List.UseCases/"]
COPY ["Directory.Packages.props", "./"]
COPY ["Directory.Build.props", "./"]

RUN dotnet restore "./src/ShoppingList.List.Web/ShoppingList.List.Web.csproj"
COPY . .
WORKDIR "/src/src/ShoppingList.List.Web"
RUN dotnet build "./ShoppingList.List.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ShoppingList.List.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShoppingList.List.Web.dll"]
