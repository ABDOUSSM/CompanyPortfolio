# Step 1: Base image for runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Step 2: Build environment with SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CompanyPortfolio/CompanyPortfolio.csproj", "CompanyPortfolio/"]
RUN dotnet restore "CompanyPortfolio/CompanyPortfolio.csproj"
COPY . .
WORKDIR "/src/CompanyPortfolio"
RUN dotnet build "CompanyPortfolio.csproj" -c Release -o /app/build

# Step 3: Publish the application
FROM build AS publish
RUN dotnet publish "CompanyPortfolio.csproj" -c Release -o /app/publish

# Step 4: Final image that will run the app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CompanyPortfolio.dll"]
