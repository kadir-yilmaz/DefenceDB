# Multi-stage build for DefenceDB ASP.NET Core 10 application

# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

# --- CSS / Tailwind Derlemesi İçin Node.js Kurulumu ---
RUN apt-get update && apt-get install -y curl \
    && curl -sL https://deb.nodesource.com/setup_20.x | bash - \
    && apt-get install -y nodejs \
    && rm -rf /var/lib/apt/lists/*
# ------------------------------------------------------

WORKDIR /src

# Proje ve Solution dosyalarını kopyala
COPY ["DefenceDB.slnx", "."]
COPY ["DefenceDB.WebUI/DefenceDB.WebUI.csproj", "DefenceDB.WebUI/"]
COPY ["DefenceDB.BLL/DefenceDB.BLL.csproj", "DefenceDB.BLL/"]
COPY ["DefenceDB.DAL/DefenceDB.DAL.csproj", "DefenceDB.DAL/"]
COPY ["DefenceDB.EL/DefenceDB.EL.csproj", "DefenceDB.EL/"]

# .NET bağımlılıklarını yükle
RUN dotnet restore "DefenceDB.slnx"

# Tüm kaynak kodları içeri al
COPY . .

# --- Eğer WebUI içinde package.json varsa CSS'i derle ---
WORKDIR "/src/DefenceDB.WebUI"
RUN if [ -f "package.json" ]; then npm install && npm run build; fi
WORKDIR /src
# ------------------------------------------------------

# Çözümü derle
RUN dotnet build "DefenceDB.slnx" -c Release -o /app/build

# Stage 2: Publish (Yayınlama)
FROM build AS publish
RUN dotnet publish "DefenceDB.WebUI/DefenceDB.WebUI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime (Canlı Ortam)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Sağlık kontrolü (Healthcheck) için curl kur
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Publish edilen tertemiz dosyaları runtime aşamasına kopyala
COPY --from=publish /app/publish .

EXPOSE 8080

HEALTHCHECK --interval=30s --timeout=10s --start-period=60s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "DefenceDB.WebUI.dll"]
CMD ["--urls", "http://+:8080"]