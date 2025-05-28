# Backend

## Technical Requirements
- .NET 9
- ASP.NET Core Web API
- File system storage (can be extended to cloud storage)
- Docker for MySql

## Get started

```bash
git clone https://github.com/TzuChaoHuang/Backend.git
cd Backend
git dotnet restore
git dotnet run
```

## Build MySql on Docker

1. Build MySql on Docker

```bash
docker run --name mysql-container -e MYSQL_ROOT_PASSWORD=your_password -d -p 3306:3306 mysql:latest
```

