#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["FunctionalAPI.API/FunctionalAPI.API.csproj", "FunctionalAPI.API/"]
RUN dotnet restore "FunctionalAPI.API/FunctionalAPI.API.csproj"
COPY . .
WORKDIR "/src/FunctionalAPI.API"
RUN dotnet build "FunctionalAPI.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FunctionalAPI.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FunctionalAPI.API.dll"]