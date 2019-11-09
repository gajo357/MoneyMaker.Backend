FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["MoneyMaker.Api.Canopy/MoneyMaker.Api.Canopy.csproj", "MoneyMaker.Api.Canopy/"]
RUN dotnet restore "MoneyMaker.Api.Canopy/MoneyMaker.Api.Canopy.csproj"
COPY . .
WORKDIR "/src/MoneyMaker.Api.Canopy"
RUN dotnet build "MoneyMaker.Api.Canopy.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MoneyMaker.Api.Canopy.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MoneyMaker.Api.Canopy.dll"]