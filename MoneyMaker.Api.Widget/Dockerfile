FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["MoneyMaker.Api.Widget/MoneyMaker.Api.Widget.csproj", "MoneyMaker.Api.Widget/"]
RUN dotnet restore "MoneyMaker.Api.Widget/MoneyMaker.Api.Widget.csproj"
COPY . .
WORKDIR "/src/MoneyMaker.Api.Widget"
RUN dotnet build "MoneyMaker.Api.Widget.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MoneyMaker.Api.Widget.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MoneyMaker.Api.Widget.dll"]