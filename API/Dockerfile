# Imagen SDK .NET 8 para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiamos los archivos y restauramos dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiamos el resto y publicamos la app
COPY . ./
RUN dotnet publish -c Release -o out

# Imagen final de ejecución
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/out .

# Puerto
EXPOSE 7863

# Variable de entorno 
ENV NOMBRE_TIENDA="Tetería"

# Ejecutar la aplicación
ENTRYPOINT ["dotnet", "TeaShop.dll"]