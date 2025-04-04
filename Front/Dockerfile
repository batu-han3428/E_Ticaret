FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=3502
EXPOSE 3502

COPY Front/Front.csproj ./Front/
COPY Common/Common.csproj ./Common/
RUN dotnet restore ./Front/Front.csproj

COPY . .

ENV ASPNETCORE_ENVIRONMENT Development

CMD ["dotnet", "watch", "run", "--project", "Front/Front.csproj", "--urls", "http://0.0.0.0:3502"]