dev:
	dotnet watch --project src\Products.Api\Products.Api.csproj

start:
	dotnet run --project src\Products.Api\Products.Api.csproj

test:
	dotnet test src\Products.Tests\Products.Tests.csproj /p:CollectCoverage=true