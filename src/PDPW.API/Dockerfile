# Estágio de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar arquivos de projeto
COPY ["src/PDPW.API/PDPW.API.csproj", "PDPW.API/"]
COPY ["src/PDPW.Application/PDPW.Application.csproj", "PDPW.Application/"]
COPY ["src/PDPW.Domain/PDPW.Domain.csproj", "PDPW.Domain/"]
COPY ["src/PDPW.Infrastructure/PDPW.Infrastructure.csproj", "PDPW.Infrastructure/"]

# Restaurar dependências
RUN dotnet restore "PDPW.API/PDPW.API.csproj"

# Copiar todo o código fonte
COPY src/ .

# Build da aplicação
WORKDIR "/src/PDPW.API"
RUN dotnet build "PDPW.API.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "PDPW.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio final - runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Instalar ferramentas necessárias (curl para health check)
RUN apt-get update && apt-get install -y \
    curl \
    && rm -rf /var/lib/apt/lists/*

EXPOSE 80
EXPOSE 443

# Copiar arquivos publicados
COPY --from=publish /app/publish .

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=60s --retries=3 \
  CMD curl -f http://localhost:80/health || exit 1

# Migrations e Seeder serão executados automaticamente via Program.cs
ENTRYPOINT ["dotnet", "PDPW.API.dll"]
