# DormCare

Простий навчальний ASP.NET Core MVC додаток для керування гуртожитком — кімнати, студенти, заявки; з EF Core + SQLite та аутентифікацією (адмін/студент).

**Швидкий старт**

-   **Вимоги:** .NET 9 SDK, sqlite (необов'язково), git.
-   **Побудова і запуск:**

```bash
cd DormCare
dotnet build
dotnet run --urls "http://localhost:5191"
```

-   Відкрийте http://localhost:5191 у браузері.

**Налаштування бази даних**

-   Додаток використовує SQLite за замовчуванням (`Data Source=dormcare.db`).
-   Міграції застосовуються автоматично при запуску (в `Program.cs` є `db.Database.Migrate()`).
-   Для роботи з міграціями в середовищі розробки можна використовувати design-time фабрику: [DormCare/Data/ApplicationDbContext.cs](DormCare/Data/ApplicationDbContext.cs)

**Облікові записи**

-   За замовчуванням у базі створюється адмін: **admin@dorm.local / admin123** (ініціалізується через `OnModelCreating`).

**Тести**

-   Юніт тести (xUnit + EF InMemory): проект `tests/DormCare.Tests`.
-   Інтеграційні тести (xUnit + WebApplicationFactory): проект `tests/DormCare.IntegrationTests`.
-   Запуск усіх тестів:

```bash
cd DormCare
dotnet test ../tests/DormCare.Tests/DormCare.Tests.csproj --verbosity minimal
dotnet test ../tests/DormCare.IntegrationTests/DormCare.IntegrationTests.csproj --verbosity minimal
```

Або з кореня репозиторію запустити всі тести одночасно:

```bash
dotnet test
```

**Структура проекту (коротко)**

-   `DormCare/Program.cs` — конфігурація сервера, EF, аутентифікація.
-   `DormCare/Data/ApplicationDbContext.cs` — EF Core DbContext, DbSet'и і початковий seed.
-   `DormCare/Models` — моделі: `User`, `Room`, `Student`, `Request`, view-моделі.
-   `DormCare/Controllers` — контролери для обліку, адмін-панелі та студентської частини.
-   `DormCare/Views` — Razor-шаблони (Bootstrap для стилю).
-   `tests/` — тести: `DormCare.Tests` (unit), `DormCare.IntegrationTests` (integration).

**Основні маршрути / UI**

-   Головна: `/` — кнопки Login / Register.
-   Адмін: керування кімнатами `/Rooms` (через роль Admin), студенти `/Admin/Students`.
-   Студент: власна кабінет/заявки `/StudentHome` та `/Student/Requests`.
