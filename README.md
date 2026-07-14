# Todo App

Простий ToDo-додаток з ASP.NET Core API та Angular UI.

## Що вже перевірено

На цій машині проєкт успішно:

- збирається для бекенду: `dotnet build backend/TodoApp.Api.slnx`
- збирається для фронтенду: `npm run build` у папці `frontend/todo-app`
- запускається локально:
  - API: `https://localhost:7272/swagger`
  - UI: `http://localhost:4200`

## Перед запуском

Потрібно мати встановлене:

- .NET 8 SDK
- Node.js 20+
- SQL Server LocalDB (для поточної конфігурації підключення)

## Як запустити

### 1. Бекенд

```bash
cd backend/TodoApp.Api
 dotnet restore
 dotnet run --launch-profile https
```

API буде доступне на:

- https://localhost:7272
- http://localhost:5241
- Swagger: https://localhost:7272/swagger

### 2. Фронтенд

```bash
cd frontend/todo-app
npm install
npm start
```

UI буде доступне на:

- http://localhost:4200
