# SmartFlow Tracker

AI-powered task board with clean architecture, OpenAI integration, and Telegram bot control. Built with .NET 8, Angular 19, CQRS, Dapper, PostgreSQL, and Docker.

---

## 📦 Project Structure

SmartFlow-Tracker/
├── src/
│ ├── SmartFlow.Tracker.sln # Solution file
│ ├── SmartFlow.Tracker.Api/ # ASP.NET Core API
│ ├── SmartFlow.Tracker.Application/ # UseCases, CQRS, DTOs
│ ├── SmartFlow.Tracker.Domain/ # Entities, enums
│ ├── SmartFlow.Tracker.Infrastructure/ # Dapper & DB setup
│ ├── SmartFlow.Tracker.AI/ # OpenAI integration
│ ├── SmartFlow.Tracker.Bots/ # Telegram bot
│ └── SmartFlow.Tracker.Migrations/ # DbUp-based migration runner
│ └── Migrations/
│ └── 001_CreateTasksTable.sql
│
├── db/
│ └── init.sql # Optional DB init for Docker
├── docker-compose.yml
└── README.md


---

## 🛠️ Technologies

| Layer           | Tech Stack                                  |
|-----------------|----------------------------------------------|
| Frontend        | Angular 19, Signals, TailwindCSS             |
| Backend         | .NET 8, CQRS, Clean Architecture             |
| Database        | PostgreSQL (Docker)                          |
| Persistence     | Dapper                                       |
| AI              | OpenAI / Azure OpenAI API                    |
| Bots            | Telegram Bot API                             |
| DevOps          | Docker, Docker Compose, DbUp (for migrations)|

---

## 🚀 Getting Started

### 1. Clone the repo

```bash
git clone https://github.com/your-username/SmartFlow-Tracker.git
cd SmartFlow-Tracker
docker-compose up --build

API: http://localhost:5000

Swagger UI: http://localhost:5000/swagger

PostgreSQL: localhost:5432 (user: postgres, password: postgres)