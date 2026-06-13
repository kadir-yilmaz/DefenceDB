# Multi-stage build for DefenceDB ASP.NET Core 8 application

# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy solution and project files
COPY ["DefenceDB.slnx", "."]
COPY ["DefenceDB.WebUI/DefenceDB.WebUI.csproj", "DefenceDB.WebUI/"]
COPY ["DefenceDB.BLL/DefenceDB.BLL.csproj", "DefenceDB.BLL/"]
COPY ["DefenceDB.DAL/DefenceDB.DAL.csproj", "DefenceDB.DAL/"]
COPY ["DefenceDB.EL/DefenceDB.EL.csproj", "DefenceDB.EL/"]

# Restore dependencies
RUN dotnet restore "DefenceDB.slnx"

# Copy all source code
COPY . .

# Build the solution
RUN dotnet build "DefenceDB.slnx" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "DefenceDB.WebUI/DefenceDB.WebUI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy published files from publish stage
COPY --from=publish /app/publish .

# Expose port (adjust if needed)
EXPOSE 8080

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

# Start the application
ENTRYPOINT ["dotnet", "DefenceDB.WebUI.dll"]
CMD ["--urls", "http://+:8080"]
