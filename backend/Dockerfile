# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["ProjetoEcommerce.Api/ProjetoEcommerce.Api.csproj", "ProjetoEcommerce.Api/"]
COPY ["ProjetoEcommerce.Application/ProjetoEcommerce.Application.csproj", "ProjetoEcommerce.Application/"]
COPY ["ProjetoEcommerce.Domain/ProjetoEcommerce.Domain.csproj", "ProjetoEcommerce.Domain/"]
COPY ["ProjetoEcommerce.Infra.Data/ProjetoEcommerce.Infra.Data.csproj", "ProjetoEcommerce.Infra.Data/"]
COPY ["ProjetoEcommerce.Infra.IoC/ProjetoEcommerce.Infra.IoC.csproj", "ProjetoEcommerce.Infra.IoC/"]
COPY ["ProjetoEcommerce.Infra.MessageQueue/ProjetoEcommerce.Infra.MessageQueue.csproj", "ProjetoEcommerce.Infra.MessageQueue/"]
COPY ["ProjetoEcommerce.Infra.Cache/ProjetoEcommerce.Infra.Cache.csproj", "ProjetoEcommerce.Infra.Cache/"]
COPY ["ProjetoEcommerce.Infra.Cloud/ProjetoEcommerce.Infra.Cloud.csproj", "ProjetoEcommerce.Infra.Cloud/"]

RUN dotnet restore "ProjetoEcommerce.Api/ProjetoEcommerce.Api.csproj"

COPY . .
WORKDIR "/src/ProjetoEcommerce.Api"
RUN dotnet build "ProjetoEcommerce.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "ProjetoEcommerce.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "ProjetoEcommerce.Api.dll"]
