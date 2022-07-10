#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Toolsfactory.Home.Adapters.MultiHost/Toolsfactory.Home.Adapters.MultiHost.csproj", "src/Toolsfactory.Home.Adapters.MultiHost/"]
COPY ["src/Toolsfactory.Home.Adapters.Weather.WeatherLogger2/Toolsfactory.Home.Adapters.Weather.WeatherLogger2.csproj", "src/Toolsfactory.Home.Adapters.Weather.WeatherLogger2/"]
COPY ["libs/Toolsfactory.Protocols.Homie/src/Toolsfactory.Protocols.Homie/Toolsfactory.Protocols.Homie.csproj", "libs/Toolsfactory.Protocols.Homie/src/Toolsfactory.Protocols.Homie/"]
COPY ["src/Toolsfactory.Home.Adapters.Common/Toolsfactory.Home.Adapters.Common.csproj", "src/Toolsfactory.Home.Adapters.Common/"]
COPY ["libs/Tiveria.Common.BasicHttpServer/src/Tiveria.Common.BasicHttpServer/Tiveria.Common.BasicHttpServer.csproj", "libs/Tiveria.Common.BasicHttpServer/src/Tiveria.Common.BasicHttpServer/"]
COPY ["src/Toolsfactory.Home.Adapters.Heating.Wolf.Service/Toolsfactory.Home.Adapters.Heating.Wolf.csproj", "src/Toolsfactory.Home.Adapters.Heating.Wolf.Service/"]
COPY ["src/Toolsfactory.Home.Adapters.GasPrices.TankerKoenig.Service/Toolsfactory.Home.Adapters.Gasprices.TankerKoenig.csproj", "src/Toolsfactory.Home.Adapters.GasPrices.TankerKoenig.Service/"]
COPY ["src/Toolsfactory.Home.Adapters.GasPrices.Interfaces/Toolsfactory.Home.Adapters.Gasprices.Interfaces.csproj", "src/Toolsfactory.Home.Adapters.GasPrices.Interfaces/"]
COPY ["src/Toolsfactory.Home.Adapters.Garbage.Awido.Service/Toolsfactory.Home.Adapters.Garbage.Awido.csproj", "src/Toolsfactory.Home.Adapters.Garbage.Awido.Service/"]
COPY ["src/Toolsfactory.Home.Adapters.Powermeter.D0/Toolsfactory.Home.Adapters.Powermeter.D0.csproj", "src/Toolsfactory.Home.Adapters.Powermeter.D0/"]
COPY ["libs/Toolsfactory.Protocols.D0/src/Toolsfactory.Protocols.D0/Toolsfactory.Protocols.D0.csproj", "libs/Toolsfactory.Protocols.D0/src/Toolsfactory.Protocols.D0/"]
RUN dotnet restore "src/Toolsfactory.Home.Adapters.MultiHost/Toolsfactory.Home.Adapters.MultiHost.csproj"
COPY . .
WORKDIR "/src/src/Toolsfactory.Home.Adapters.MultiHost"
RUN dotnet build "Toolsfactory.Home.Adapters.MultiHost.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Toolsfactory.Home.Adapters.MultiHost.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "multihost.host.dll"]