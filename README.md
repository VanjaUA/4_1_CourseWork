# Warehouse Inventory System (Coursework)

Система складського обліку: бекенд на ASP.NET Core + Supabase + фронтенд на React

## Структура репозиторію
- `backend/` — ASP.NET Core Web API
- `frontend/warehouse-frontend/` — React веб-інтерфейс

## Налаштування БД (Supabase)
Потрібно для початку створити проєкт у Supabase.
**connection string** (краще **Session pooler**, якщо Direct IPv6-only) вставити у файл:
`backend/src/Warehouse.Api/appsettings.Development.json`

Приклад формату (Npgsql):
```json
"ConnectionStrings": {
  "Postgres": "Host=...;Port=...;Database=postgres;Username=...;Password=...;SSL Mode=Require;Trust Server Certificate=true;Search Path=public"
}

## Запуск проєкту

1) бекенд

cd backend
dotnet run --project src/Warehouse.Api

(http://localhost:5259/swagger/index.html)

2) фронтенд

cd frontend/warehouse-frontend
npm.cmd install
npm.cmd run dev

(http://localhost:5173/)


ADMIN ROLE in backend\src\Warehouse.Api\appsettings.Development.json

  "AdminSeed": {
    "Email": "admin@warehouse.com",
    "Password": "admin"
  }