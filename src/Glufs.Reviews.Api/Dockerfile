#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Glufs.Reviews.Api/Glufs.Reviews.Api.csproj", "src/Glufs.Reviews.Api/"]
RUN dotnet restore "src/Glufs.Reviews.Api/Glufs.Reviews.Api.csproj"
COPY . .
WORKDIR "/src/src/Glufs.Reviews.Api"
RUN dotnet build "Glufs.Reviews.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Glufs.Reviews.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Glufs.Reviews.Api.dll"]