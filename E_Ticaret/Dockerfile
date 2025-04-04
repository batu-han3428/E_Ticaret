#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=3500
EXPOSE 3500
EXPOSE 3501

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["E_Ticaret/E_Ticaret.csproj", "E_Ticaret/"]
RUN dotnet restore "./E_Ticaret/E_Ticaret.csproj"
COPY . .
WORKDIR "/src/E_Ticaret"
RUN dotnet build "./E_Ticaret.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./E_Ticaret.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "E_Ticaret.dll"]