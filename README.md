# Progetto .NET8 con Redis, Docker e MySQL
Questo progetto è un'applicazione .NET che utilizza Redis e MySQL come servizi di memorizzazione dei dati, gestiti tramite Docker. L'applicazione è un'esempio di come configurare e utilizzare Redis e MySQL all'interno di un progetto .NET e come gestire l'ambiente di sviluppo utilizzando Docker.

## Requisiti
Docker Desktop installato sul tuo sistema.
.NET SDK installato sul tuo sistema.
Conoscenza base di Docker, .NET e SQL.
Configurazione dell'Ambiente

### Creare un nuovo progetto .NET:

Assicurarsi di avere il .NET SDK installato sul proprio sistema.
Aprire un terminale e eseguire il seguente comando per creare un nuovo progetto .NET:

## Pachetti da installare
    dotnet add package MySql.EntityFrameworkCore
    dotnet add package Microsoft.EntityFrameworkCore
    dotnet add package Microsoft.EntityFrameworkCore.Tools
    dotnet add package Microsoft.EntityFrameworkCore.Design
    dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis

### Configurare Docker Compose:
Creare un file docker-compose.yml nella radice del progetto e configurare i servizi di MySQL e Redis secondo le proprie esigenze.

    version: '3.4'

    services:
      redisdemo3:
        image: ${DOCKER_REGISTRY-}redisdemo3
        build:
          context: .
          dockerfile: RedisDemo3/Dockerfile
      mysqldb:
        image: mysql
        restart: always
        ports:
          - "3306:3306"
        volumes:
          - ./store/:/var/lib/mysql   # Corretto l'indentazione e aggiunto il prefisso '/'
        environment:
          - MYSQL_ROOT_PASSWORD=root  
          - MYSQL_DATABASE=redisdemodb
          - MYSQL_USER=userdemo
          - MYSQL_PASSWORD=12345     # Corretto il nome della variabile d'ambiente
      rediscache:
        image: redis:latest
        restart: always
        ports:
          - '6379:6379'

### Configurare il Progetto .NET:
Aprire il file appsettings.json nel progetto .NET e configurare la stringa di connessione per MySQL e Redis.

    {
    "ConnectionStrings": {
      "MySqlConnection": "server=mysqldb;database=redisdemodb;user=userdemo;password=12345;",
      "RedisCache": "rediscache:6379"
    },
    "Logging": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft.AspNetCore": "Warning"
      }
    },
    "AllowedHosts": "*"
    }
    
##  Se sei sul VS Code
Assicurarsi che i servizi di MySQL e Redis siano avviati correttamente.

### Build e Avvio del Progetto
    dotnet build

### Avvio dei Servizi Docker:
    docker-compose up --build
