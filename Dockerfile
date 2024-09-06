# Estágio 1: Build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia o arquivo de projeto e restaura as dependências
COPY *.csproj ./ 
RUN dotnet restore

# Copia o restante do código da aplicação e publica a aplicação
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Estágio 2: Imagem de produção
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copia a aplicação publicada do estágio de build
COPY --from=build /app/publish .

# Comando para rodar a aplicação no container
ENTRYPOINT ["dotnet", "Product.Api.dll"]