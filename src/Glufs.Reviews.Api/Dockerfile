#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Glufs.Review.Api/Glufs.Review.Api.csproj", "src/Glufs.Review.Api/"]
RUN dotnet restore "src/Glufs.Review.Api/Glufs.Review.Api.csproj"
COPY . .
WORKDIR "/src/src/Glufs.Review.Api"
RUN dotnet build "Glufs.Review.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Glufs.Review.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Glufs.Review.Api.dll"]