# المرحلة الأولى: build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# ننسخ الملفات ونبني المشروع
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# المرحلة الثانية: runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

# نحدد البورت 8080 ليتوافق مع fly.toml
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "CompanyPortfolio.dll"]