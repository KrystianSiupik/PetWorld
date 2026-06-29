# PetWorld

Sklep zoologiczny z asystentem AI. Klient zadaje pytanie na czacie, a system (w architekturze **Writer-Critic**) generuje i recenzuje odpowiedź, rekomendując produkty z oferty. Rozmowy trafiają do historii.

## Funkcje

- **Chat** — klient zadaje pytanie, otrzymuje odpowiedź wraz z liczbą iteracji pętli Writer-Critic.
- **Historia** — lista wcześniejszych rozmów (data, pytanie, odpowiedź, liczba iteracji).

## Uruchomienie

Wymagany **Docker** (z Docker Compose). Aplikacja startuje jednym poleceniem.

1. Skopiuj plik z kluczem API i uzupełnij go:
   ```bash
   cp .env.example .env
   # w .env ustaw OPENAI_API_KEY=sk-...
   ```
2. Uruchom:
   ```bash
   docker compose up
   ```
3. Otwórz **http://localhost:5000**

Przy starcie aplikacja automatycznie wykonuje migracje bazy i wypełnia katalog produktów (jeśli pusty).

## Konfiguracja

| Zmienna | Opis | Domyślnie |
|---|---|---|
| `OPENAI_API_KEY` | Klucz API OpenAI (wymagany do działania chatu) | — |
| `OpenAI__Model` | Model OpenAI | `gpt-4o-mini` |
| `ConnectionStrings__DefaultConnection` | Connection string MySQL | ustawiony w `docker-compose.yml` |

Klucz API jest wczytywany ze zmiennej środowiskowej lub `appsettings.json` (`OpenAI:ApiKey`).

## Architektura

Onion / Clean Architecture — 4 projekty, zależności skierowane wyłącznie do wewnątrz:

```
Web (Blazor Server)  →  Infrastructure  →  Application  →  Domain
   composition root       EF, agenci AI      use-case'y      encje, VO
```

- **Domain** — encje, value objecty, smart enumy; zero zależności zewnętrznych.
- **Application** — porty (interfejsy), use-case'y (handlery), DTO. Nie zna frameworków.
- **Infrastructure** — EF Core + MySQL, agenci na Microsoft Agent Framework, seed katalogu.
- **Web** — Blazor Server (UI) + composition root (DI, wybór providera LLM, klucz API).

## Jak to działa

### Pętla Writer-Critic

`AskQuestionHandler` (warstwa Application) orkiestruje dwóch agentów:

1. **Writer** generuje odpowiedź dla klienta. Aby nie zmyślać, korzysta z narzędzia, które zwraca aktualny katalog produktów, i poleca wyłącznie pozycje z oferty.
2. **Critic** ocenia odpowiedź i zwraca strukturę `{ approved, feedback }`.
3. Jeśli `approved == false`, uwagi Critica trafiają z powrotem do Writera, który poprawia odpowiedź.
4. Pętla powtarza się **maksymalnie 3 razy**. Zwracana jest ostatnia wygenerowana odpowiedź wraz z liczbą iteracji; całość zapisywana jest do historii.

### Dobór produktów

Narzędzie Writera zwraca **cały katalog**, a wyboru pasujących produktów dokonuje model językowy. Przy małym katalogu jest to prostsze i stabilniejsze niż wyszukiwanie po słowach kluczowych (które przy polskiej odmianie bywa zawodne), bo model rozumie kontekst i synonimy. Dla dużego katalogu właściwym kierunkiem jest wyszukiwanie pełnotekstowe lub wektorowe (RAG) — patrz „Możliwe usprawnienia".

## Decyzje projektowe i kompromisy

- **Bogaty model domenowy** — value objecty (`Money`, `ProductName`, `IterationCount`, `CustomerQuestion`, `AssistantAnswer`) i smart enumy (`ProductCategory`, `Currency`). Niemożliwe „puste"/niepoprawne obiekty; walidacja w jednym miejscu.
- **Błędy domenowe przez `DomainException`, nie `Result<T>`** — świadomie zrezygnowano z `Result` jako zbędnej ceremonii przy tej skali.
- **Bez MediatR i AutoMapper** — przy dwóch use-case'ach to przerost; ręczne handlery i ręczny mapper. (Oba przeszły też na licencje komercyjne w 2025.)
- **Limit „max 3 iteracje" jako stała w `AskQuestionHandler`** — to polityka orkiestracji (Application), a nie reguła domeny.
- **Dobór produktów = cały katalog + filtrowanie przez LLM** — zamiast kruchego wyszukiwania po słowach; precyzję dodatkowo pilnuje Critic.
- **Mapowanie EF** — `Money` jako owned type (kolumny `PriceAmount` + `PriceCurrency`), smart enumy zapisywane jako `Name`; `Product` ma dodatkowy konstruktor dla EF (owned type nie jest wiązany przez konstruktor).
- **`EnableRetryOnFailure`** — odporność na wyścig pierwszego startu MySQL w kontenerze.
- **`IChatClient` jako abstrakcja** — wybór providera (OpenAI) i klucz API żyją tylko w composition root; Infrastructure jest provider-agnostyczna.
- **Auto-migracja + seed przy starcie** — `docker compose up` działa bez ręcznych kroków.

## Struktura projektu

```
PetWorld/
├── docker-compose.yml          app + MySQL (healthcheck, port 5000)
├── Dockerfile                  multi-stage build (SDK -> aspnet runtime)
├── .env.example
├── PetWorld.slnx
└── src/
    ├── PetWorld.Domain/
    │   ├── SharedKernel/        SmartEnum, DomainException
    │   ├── Products/            Product + VO + ProductCategory/Currency
    │   └── Chat/                ChatInteraction + VO
    ├── PetWorld.Application/
    │   ├── Chat/                Abstractions, Handlers, Mapping, Models
    │   └── Products/Abstractions/   IProductRepository
    ├── PetWorld.Infrastructure/
    │   ├── Persistence/         DbContext, konfiguracje, repozytoria, seed, migracje
    │   └── Agents/              WriterAgent, CriticAgent, ProductCatalogTool
    └── PetWorld.Web/            Blazor Server: strony Chat i Historia, composition root
```

## Stack technologiczny

- .NET 10, Blazor Server
- Microsoft Agent Framework (agenci Writer/Critic, tool-calling, structured output)
- EF Core 9 + Pomelo (MySQL 8.0)
- QuickGrid (historia)
- Docker / Docker Compose

## Możliwe usprawnienia

- **Wyszukiwanie produktów dla dużego katalogu** — pełnotekstowe (MySQL FULLTEXT / Elasticsearch) lub wektorowe (embeddingi + baza wektorowa) w schemacie RAG, zamiast podawania całego katalogu do kontekstu.
- **Strumieniowanie odpowiedzi** — wyświetlanie odpowiedzi Writera na bieżąco.
- **Testy** — jednostkowe dla pętli Writer-Critic i value objectów, integracyjne dla repozytoriów.
- **Konfigurowalny provider LLM** — przełączanie OpenAI / Azure OpenAI z konfiguracji.
