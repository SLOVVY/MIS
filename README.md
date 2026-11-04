# МИС API (.NET 10 + Aspire + PostgreSQL)

Тестовое задание: API для медицинской информационной системы (пациенты, врачи, болезни).

## Стек
- .NET 10 (ASP.NET Core)
- .NET Aspire
- PostgreSQL (через Docker)
- EF Core (миграции)

## Требования для запуска
- Установленный .NET 10 SDK
- Запущенный Docker Desktop

## Как запустить
1. Откройте решение `MIS.sln` в Visual Studio.
2. Убедитесь, что стартовым проектом (Startup Project) выбран **`MIS.AppHost`**.
3. Нажмите F5 (или `dotnet run --project MIS.AppHost`).
4. Откроется Aspire Dashboard (браузер).
5. Дождитесь, пока сервис `migrations` завершит работу (статус Finished), а `api` запустится (Running).
6. В строке ресурса `api` нажмите на ссылку **Swagger UI** для тестирования эндпоинтов.
