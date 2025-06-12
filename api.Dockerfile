FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build-env

ARG NUGET_USER
ARG NUGET_PASSWORD

RUN dotnet nuget add source "https://nuget.pkg.github.com/clcl0n/index.json" --name Github --username $NUGET_USER --password $NUGET_PASSWORD --store-password-in-clear-text

WORKDIR /App

# Copy 
COPY ./src .

# Build Api
WORKDIR /App/Cms.SitemapService/src/Api/src

# Build and publish a release
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine as runtime-env

WORKDIR /App

COPY --from=build-env /App/Cms.SitemapService/src/Api/src/out .

RUN apk update
RUN apk add bash

ENTRYPOINT ["dotnet", "Cms.SitemapService.Api.dll"]
