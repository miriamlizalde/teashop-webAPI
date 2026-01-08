# 1. Usamos la imagen del SDK de .NET 8 para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# 2. Copiamos los archivos y restauramos dependencias
COPY *.csproj ./
RUN dotnet restore

# 3. Copiamos el resto y publicamos la app
COPY . ./
RUN dotnet publish -c Release -o out

# 4. Imagen final de ejecución
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/out .

# --- REQUISITOS ACTIVIDAD ---
# Puerto: Usa las 4 últimas cifras de tu usuario (Ej: 4569)
EXPOSE 4569

# Variable de entorno (Punto Extra)
ENV APP_MODE="Production"
ENV STORE_NAME="TeaShop_Miriam"

# Ejecutar la aplicación
ENTRYPOINT ["dotnet", "TeaShop.dll"]