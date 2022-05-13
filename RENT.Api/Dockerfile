#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RENT.Api/RENT.Api.csproj", "RENT.Api/"]
RUN dotnet restore "RENT.Api/RENT.Api.csproj"
COPY . .
WORKDIR "/src/RENT.Api"
RUN dotnet build "RENT.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RENT.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RENT.Api.dll"]