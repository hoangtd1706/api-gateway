#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Ecoba.ApiGateway/Ecoba.ApiGateway.csproj", "Ecoba.ApiGateway/"]
RUN dotnet restore "Ecoba.ApiGateway/Ecoba.ApiGateway.csproj"
COPY . .
WORKDIR "/src/Ecoba.ApiGateway"
RUN dotnet build "Ecoba.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ecoba.ApiGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ecoba.ApiGateway.dll"]
