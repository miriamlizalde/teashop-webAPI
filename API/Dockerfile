FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Models/TeaShop.Models.csproj     Models/
COPY Data/TeaShop.Data.csproj         Data/
COPY Business/TeaShop.Business.csproj Business/
COPY API/TeaShop.API.csproj           API/

RUN dotnet restore API/TeaShop.API.csproj

COPY Models/    Models/
COPY Data/      Data/
COPY Business/  Business/
COPY API/       API/

RUN dotnet publish API/TeaShop.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "TeaShop.API.dll"]