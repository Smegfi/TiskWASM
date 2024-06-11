## BUILD container
FROM bitnami/dotnet-sdk:6 as build

WORKDIR /app-build
COPY . .

RUN dotnet publish -c Release -o release

## RELEASE container
FROM alpine
RUN apk add aspnetcore6-runtime
RUN apk update
RUN apk upgrade

WORKDIR /app
COPY --from=build /app-build/release .
WORKDIR /app/Data
COPY ./TiskWASM/Server/Data/. .
EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000

WORKDIR /app
CMD ["dotnet","TiskWASM.Server.dll"]