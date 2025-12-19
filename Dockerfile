# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copiar solution e projetos
COPY ["PDPW.sln", "./"]
COPY ["src/PDPW.API/PDPW.API.csproj", "src/PDPW.API/"]
COPY ["src/PDPW.Application/PDPW.Application.csproj", "src/PDPW.Application/"]
COPY ["src/PDPW.Domain/PDPW.Domain.csproj", "src/PDPW.Domain/"]
COPY ["src/PDPW.Infrastructure/PDPW.Infrastructure.csproj", "src/PDPW.Infrastructure/"]

# Restore
RUN dotnet restore "PDPW.sln"

# Copiar todo o código
COPY . .

# Build
WORKDIR "/source/src/PDPW.API"
RUN dotnet build "PDPW.API.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "PDPW.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Install dotnet-ef tool
RUN dotnet tool install --global dotnet-ef --version 8.0.0
ENV PATH="${PATH}:/root/.dotnet/tools"

# Copiar arquivos publicados
COPY --from=publish /app/publish .

# Entrypoint
ENTRYPOINT ["dotnet", "PDPW.API.dll"]
