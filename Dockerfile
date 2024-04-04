FROM bitnami/dotnet-sdk:6

RUN mkdir -p /app
RUN mkdir -p /app/src
RUN mkdir -p /app/prod

COPY . /app/src

WORKDIR /app/src

RUN dotnet publish -c Release -o /app/prod

WORKDIR /app/prod

EXPOSE 5000/tcp
EXPOSE 5000/udp

CMD ["dotnet","TiskWASM.Server.dll"]
