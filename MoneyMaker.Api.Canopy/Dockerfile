FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

# Install Chrome
RUN apt-get update
RUN apt-get install -y wget
RUN wget https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb
RUN dpkg -i google-chrome-stable_current_amd64.deb; apt-get -fy install

## install chromedriver
RUN apt-get install -yqq unzip
RUN wget -O /tmp/chromedriver.zip http://chromedriver.storage.googleapis.com/`curl -sS chromedriver.storage.googleapis.com/LATEST_RELEASE`/chromedriver_linux64.zip
RUN unzip /tmp/chromedriver.zip chromedriver -d /usr/local/bin/

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