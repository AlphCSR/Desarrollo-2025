# Utiliza una imagen multi-stage para optimizar el tamaño final
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia los archivos de solución y restaura dependencias
COPY UsersMS.sln ./
COPY UsersMS/UsersMS.csproj UsersMS/
COPY UsersMS.Application/UsersMS.Application.csproj UsersMS.Application/
COPY UsersMS.Commons/UsersMS.Commons.csproj UsersMS.Commons/
COPY UsersMS.Core/UsersMS.Core.csproj UsersMS.Core/
COPY UsersMS.Domain/UsersMS.Domain.csproj UsersMS.Domain/
COPY UsersMS.Infrastructure/UsersMS.Infrastructure.csproj UsersMS.Infrastructure/
COPY UsersMS.Test/UsersMS.Test.csproj UsersMS.Test/
RUN dotnet restore "UsersMS/UsersMS.csproj"

# Copia el resto de los archivos y compila
COPY . .
WORKDIR "/src/UsersMS"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Imagen final
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "UsersMS.dll"]
