#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Muhsin3Categories.API/Muhsin3Categories.API.csproj", "Muhsin3Categories.API/"]
RUN dotnet restore "Muhsin3Categories.API/Muhsin3Categories.API.csproj"
COPY . .
WORKDIR "/src/Muhsin3Categories.API"
RUN dotnet build "Muhsin3Categories.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Muhsin3Categories.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Muhsin3Categories.API.dll"]