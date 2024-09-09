
# Products Service

API para realização do CRUD de Produtos.

## Funcionalidades:

- Criar um produto
  - O valor do produto não pode ser negativo
- Atualizar um produto
- Deletar um produto
- Listar os produtos
  - Visualizar um produto específico
  - Ordenar os produtos por diferentes campos
  - Buscar produto pelo nome
## Stack utilizada

- **.Net 8**: 
- **PostgreSQL 15**
- **Docker Compose**: Caso não tenha PostgreSQL e não queira utilizar o banco de dados em memória. Guia instalação [aqui](https://docs.docker.com/desktop/install/windows-install/).
- **Entity Framework**: Utilizando code-first.


## Variáveis de Ambiente

Para rodar esse projeto, você vai precisar adicionar as seguintes variáveis de ambiente no arquivo *src\Products.Api\Properties\launchSettings.json*.

`DB_PASSWORD`: Senha do banco de dados

`DB_TYPE`: Tipo dos banco. Tipos aceitáveis `memory` ou `pgsql`


## Rodando localmente

Caso não tenha o Make instalado no computador execute o seguinte comando:

```bash
  choco install make
```

Clone o projeto

```bash
  git clone https://github.com/blasther12/products.git
```

Entre no diretório do projeto

```bash
  cd products
```

Caso queira usar o PostgreSQL e não tenha ele instalado executar

```bash
  docker compose up
```

Inicie o servidor com Make

```bash
  make dev
```

Inicie o servidor sem Make

```bash
  dotnet watch --project src\Products.Api\Products.Api.csproj
```

## Documentação da API

Executar os request dentro do arquivo `src\Products.Api\Products.Api.http`.

Ou acessando Swagger da aplicação acessando: http://localhost:5258/swagger/index.html

![image](https://github.com/user-attachments/assets/a99c5fd9-c4ec-4ea5-a763-a574111721ce)

## Rodando os testes

Para rodar os testes, rode o seguinte comando

```bash
  make test
```

ou 

```bash
  dotnet test src\Products.Tests\Products.Tests.csproj /p:CollectCoverage=true
```
## Licença

[MIT](https://choosealicense.com/licenses/mit/)

