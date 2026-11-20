# Flashcards

Karteikarten-Webanwendung mit ASP.NET Core und .NET 10. Karten liegen in thematischen **Boxen** (z. B. „Netzwerktechnik“) und werden nach einer festen Intervallregel wiederholt.

> **Status:** in Entwicklung (MVP). Domain, Datenbankschema und Repositories stehen, API-Endpunkte, Weboberfläche und Auth-Dienst folgen.

## Funktionen (MVP)

- Registrierung und Login über einen eigenen Auth-Dienst (JWT, RS256)
- Boxen anlegen, umbenennen und löschen
- Karten mit Vorder- und Rückseite verwalten
- Lernmodus: fällige Karten abfragen, Antwort aufdecken, „gewusst / nicht gewusst“ markieren
- Jeder Nutzer sieht und ändert nur seine eigenen Daten

## Wiederholungsregel

Intervalle in Tagen: **1 → 3 → 5 → 7 → 14 → 21**

- Richtig beantwortet: eine Stufe weiter, nächste Fälligkeit = heute + Intervall
- Falsch beantwortet: zurück auf Anfang, Karte kommt in derselben Sitzung erneut
- Der Tag wird serverseitig in `Europe/Berlin` bestimmt

## Architektur

Die Solution folgt einer Schichtenarchitektur. Fachlogik, Datenzugriff und Oberfläche sind getrennt, die Authentifizierung läuft als eigener Dienst.

### Überblick

```
                     ┌──────────────────┐
  Login ───────────► │ Flashcards.Auth  │  ASP.NET Core Identity, eigene DB
                     └────────┬─────────┘
                              │ JWT (RS256)
                              ▼
┌──────────┐  HTTP   ┌──────────────────┐  Bearer-Token  ┌──────────────────┐
│ Browser  │ ──────► │ Flashcards.Web   │ ─────────────► │ Flashcards.Api   │
└──────────┘         │ MVC, JWT im      │                │ REST, prüft Token│
                     │ HttpOnly-Cookie  │                │ und Ownership    │
                     └──────────────────┘                └───┬──────────┬───┘
                                                             │          │ nur DI-
                                                             ▼          ▼ Registrierung
                                           ┌──────────────────┐   ┌──────────────────┐
                                           │ Application      │ ◄─│ Infrastructure   │
                                           │ Use Cases,       │   │ EF Core, Repos,  │
                                           │ Interfaces       │   │ Migrationen      │
                                           └────────┬─────────┘   └────────┬─────────┘
                                                    ▼                     ▼
                                           ┌──────────────────┐   ┌──────────────────┐
                                           │ Domain           │   │ SQL Server       │
                                           │ Entitäten, Regeln│   │ (LocalDB)        │
                                           └──────────────────┘   └──────────────────┘
```

### Projekte

| Projekt | Aufgabe |
|---|---|
| `Flashcards.Web` | Weboberfläche (ASP.NET Core MVC: Controller, Views, ViewModels). Spricht nur per `HttpClient` mit der API |
| `Flashcards.Api` | REST-API für Boxen, Karten und Lernmodus. Validiert das Token, prüft die Ownership und ist Composition Root (hier werden alle Teile per Dependency Injection zusammengesteckt) |
| `Flashcards.Auth` | Registrierung und Login, stellt signierte JWTs aus. Eigene Identity-Datenbank |
| `Flashcards.Application` | Use Cases / Services und Repository-Interfaces |
| `Flashcards.Domain` | Entitäten (`BoxModel`, `CardModel`) und Geschäftsregeln wie die Wiederholungslogik |
| `Flashcards.Infrastructure` | `FlashcardsDbContext`, Fluent-API-Konfiguration, Migrationen, Repository-Implementierungen |
| `Flashcards.Contracts` | DTOs für Requests und Responses zwischen Web und API |
| `*.Tests` | Unit-Tests (Domain, Application) und Integrationstests (API), jeweils xUnit |

### Abhängigkeiten

```
Api            → Application → Domain
Infrastructure → Application → Domain
Api            → Infrastructure   (nur für die Registrierung in Program.cs)
Web            → Contracts
```

- Die Domain kennt weder EF Core noch ASP.NET Core.
- Die Controller der API arbeiten nur gegen Application, nie direkt gegen Infrastructure.
- Repository-Interfaces liegen in Application, die Implementierungen sind `internal` in Infrastructure.
- Die Weboberfläche greift nie direkt auf die Datenbank zu und kennt keine Entitäten.

### Authentifizierung und Autorisierung

| | Zuständig | Umsetzung |
|---|---|---|
| **Authentifizierung** („Wer bist du?“) | `Flashcards.Auth` | prüft Benutzername und Passwort, stellt ein JWT aus |
| **Autorisierung** („Was darfst du?“) | `Flashcards.Api` / Application | liest die `UserId` aus dem Token und prüft bei jeder Operation, ob die Box bzw. Karte diesem Nutzer gehört |

Der Auth-Dienst signiert mit dem privaten Schlüssel, die API prüft nur mit dem öffentlichen und kann selbst keine Tokens ausstellen. In der Web-App liegt das Token in einem HttpOnly-Cookie (für JavaScript nicht lesbar), Formulare sind per Antiforgery gegen CSRF geschützt.

### Drei Arten von Modellen

| Art | Ort | Zweck |
|---|---|---|
| Entität | `Flashcards.Domain` | Fachdaten und Regeln, wird gespeichert |
| DTO | `Flashcards.Contracts` | Datenform zwischen Web und API |
| ViewModel | `Flashcards.Web` | Datenform für genau eine Seite |

### Ordnerstruktur

```
Flashcards/
├── Flashcards.Api/
├── Flashcards.Auth/
├── Flashcards.Web/
├── Flashcards.Application/
│   └── Abstractions/        IBaseRepository, IBoxRepository, ICardRepository
├── Flashcards.Domain/
│   └── Model/               BaseModel, BoxModel, CardModel
├── Flashcards.Infrastructure/
│   ├── Database/            FlashcardsDbContext, DependencyInjection
│   ├── Migrations/
│   └── Repositories/        BoxRepository, CardRepository
├── Flashcards.Contracts/
├── Flashcards.Domain.Tests/
├── Flashcards.Application.Tests/
├── Flashcards.Api.IntegrationTests/
└── docs/                    Architekturmodell (Visual Paradigm)
```

## Tech-Stack

.NET 10 · ASP.NET Core MVC & Web API · EF Core · SQL Server (LocalDB) · ASP.NET Core Identity · JWT (RS256) · OpenAPI · xUnit

## Lokal starten

Voraussetzungen: .NET 10 SDK, SQL Server LocalDB, `dotnet-ef`

```bash
git clone https://github.com/Eckerslab/Flashcards.git
cd Flashcards
dotnet ef database update --project Flashcards.Infrastructure --startup-project Flashcards.Api
dotnet run --project Flashcards.Api
```

## Hinweis zur Authentifizierung

Der Auth-Dienst ist bewusst selbst gebaut, um JWT, Signaturprüfung und die Trennung von Authentifizierung und Autorisierung praktisch zu lernen. In Produktion würde ich einen etablierten Anbieter wie Keycloak oder Microsoft Entra ID einsetzen.
