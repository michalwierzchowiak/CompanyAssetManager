# Instrukcja uruchomienia systemu rozproszonego

Niniejszy dokument opisuje proces instalacji środowiska, konfiguracji bazy danych oraz uruchamiania systemu rozproszonego.

---

## 1. Wymagania wstępne

Aby uruchomić projekt, na komputerze muszą być zainstalowane następujące narzędzia:

- **.NET SDK 8.0** (lub nowszy)
- **Docker Desktop** (musi działać w trybie *Linux Containers*)
- **Visual Studio 2022**  
  (z obciążeniem *ASP.NET and web development*)  
  **lub** Visual Studio Code
- **Git**

---

## 2. Pobranie projektu

Sklonuj repozytorium na dysk lokalny za pomocą terminala:

```bash
git clone https://github.com/michalwierzchowiak/CompanyAssetManager.git
cd CompanyAssetManager
```

---

## 3. Uruchomienie infrastruktury (Docker)

System wymaga uruchomionych usług **SQL Server** oraz **RabbitMQ**. Zostały one zdefiniowane w pliku `docker-compose.yml`.

1. Upewnij się, że aplikacja **Docker Desktop** jest uruchomiona.
2. Otwórz terminal w głównym folderze projektu  
   (tam, gdzie znajduje się plik `.sln`).
3. Uruchom kontenery poleceniem:

```bash
docker-compose up -d
```

4. Sprawdź status kontenerów:

```bash
docker ps
```

Na liście powinny znajdować się kontenery o nazwach zawierających:

- `sqlserver`
- `rabbitmq`

---

## 4. Konfiguracja bazy danych

Projekt wykorzystuje podejście **Code First (Entity Framework Core)**.

1. Otwórz projekt w **Visual Studio**.
2. Otwórz **Package Manager Console**:
   `Narzędzia → Menedżer pakietów NuGet → Konsola menedżera pakietów`
3. W polu **Default project** wybierz projekt `API`.
4. Wykonaj komendę:

```powershell
Update-Database
```

---

## 5. Uruchomienie aplikacji

Ustaw **Multiple startup projects** i uruchom:

- `API`
- `Synchronizacja`
- `UI`

Następnie uruchom aplikację klawiszem **F5**.

---

## 6. Weryfikacja działania

- UI: https://localhost:7092
- Swagger: https://localhost:7237/swagger/index.html
- RabbitMQ: http://localhost:15672 (guest / guest)

---
