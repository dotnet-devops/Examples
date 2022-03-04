#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ApiDemo.Web/Server/ApiDemo.Web.Server.csproj", "ApiDemo.Web/Server/"]
COPY ["ApiDemo.Web/Client/ApiDemo.Web.Client.csproj", "ApiDemo.Web/Client/"]
COPY ["file://rpi/repo/Projects/Sentry/BlazorClientLibrary/BlazorClientLibrary.csproj", "file:/rpi/repo/Projects/Sentry/BlazorClientLibrary/"]
COPY ["ApiDemo.Library/ApiDemo.Library.csproj", "ApiDemo.Library/"]
RUN dotnet restore "ApiDemo.Web/Server/ApiDemo.Web.Server.csproj"
COPY . .
WORKDIR "/src/ApiDemo.Web/Server"
RUN dotnet build "ApiDemo.Web.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiDemo.Web.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiDemo.Web.Server.dll"]