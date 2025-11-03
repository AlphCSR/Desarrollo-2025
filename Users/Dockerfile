# Utiliza una imagen multi-stage para optimizar el tamaño final
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia los archivos de proyecto (.csproj) y el archivo de solución (.sln)
COPY UsersMS.sln ./
COPY */*.csproj ./
RUN for prj in $(ls -d */); do mkdir -p ${prj%%/}; mv $(basename ${prj%%/}.csproj) ${prj%%/}; done

# Restaura las dependencias para toda la solución
# Esto aprovecha el cache de Docker. Solo se volverá a ejecutar si los archivos .csproj cambian.
RUN dotnet restore "UsersMS.sln"

# Copia el resto de los archivos y compila
COPY . .
WORKDIR "/src"
RUN dotnet publish "UsersMS/UsersMS.csproj" -c Release -o /app/publish --no-restore

# Imagen final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
# Expone el puerto que la aplicación usará dentro del contenedor
EXPOSE 8080
ENTRYPOINT ["dotnet", "UsersMS.dll"]
