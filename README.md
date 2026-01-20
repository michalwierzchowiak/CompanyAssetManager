Niniejszy dokument opisuje proces instalacji środowiska, konfiguracji bazy danych oraz uruchamiania systemu rozproszonego.
1. Wymagania wstępne

Aby uruchomić projekt, na komputerze muszą być zainstalowane następujące narzędzia:

    .NET SDK 8.0 (lub nowszy)

    Docker Desktop (musi działać w trybie Linux Containers)

    Visual Studio 2022 (z obciążeniem "ASP.NET and web development") lub Visual Studio Code

    Git

2. Pobranie projektu

Sklonuj repozytorium na dysk lokalny za pomocą terminala:

git clone https://github.com/michalwierzchowiak/CompanyAssetManager.git cd CompanyAssetManager

3. Uruchomienie Infrastruktury (Docker)

System wymaga uruchomionych usług SQL Server oraz RabbitMQ. Zostały one zdefiniowane w pliku docker-compose.yml.

    Upewnij się, że aplikacja Docker Desktop jest uruchomiona.

    Otwórz terminal w głównym folderze projektu (tam, gdzie znajduje się plik .sln).

    Uruchom kontenery poleceniem:

docker-compose up -d

    Sprawdź status kontenerów:

docker ps

Na liście powinny znajdować się kontenery o nazwach zawierających sqlserver oraz rabbitmq.
4. Konfiguracja Bazy Danych

Projekt wykorzystuje podejście Code First (Entity Framework Core). Należy utworzyć strukturę tabel w bazie danych na podstawie modeli C#.

    Otwórz projekt w Visual Studio.

    Otwórz konsolę Package Manager Console (Menu: Narzędzia -> Menedżer pakietów NuGet -> Konsola menedżera pakietów).

    Upewnij się, że w polu Domyślny projekt (Default project) na górze konsoli wybrane jest: API.

    Wykonaj komendę aktualizacji bazy:

Update-Database

Jeśli komenda zakończy się sukcesem, baza danych SQL Server została utworzona i jest gotowa do pracy.

5. Uruchomienie Aplikacji

System składa się z trzech niezależnych modułów, które muszą działać jednocześnie, aby zapewnić pełną funkcjonalność.

    W Visual Studio w oknie Eksplorator Rozwiązań kliknij Prawym Przyciskiem Myszy na Rozwiązanie (Solution 'DistributedSystem').

    Wybierz opcję Ustaw projekty startowe... (Set Startup Projects).

    Zaznacz opcję Wiele projektów startowych (Multiple startup projects).

    Ustaw akcję Start dla następujących projektów:

        API (Backend API)

        Synchronizacja (Worker synchronizacji danych)

        UI (Interfejs użytkownika Blazor)

    Kliknij OK.

    Uruchom system przyciskiem F5 (lub przyciskiem Start na pasku narzędzi).

6. Weryfikacja działania

Po uruchomieniu, poszczególne moduły dostępne są pod następującymi adresami (porty mogą się różnić w zależności od konfiguracji w plikach launchSettings.json):

    Interfejs Użytkownika (UI): https://localhost:7000 (Port zmienny, otworzy się automatycznie w przeglądarce).

    Dokumentacja API (Swagger): http://localhost:5000/swagger (lub https://localhost:5001/swagger).

    Panel Zarządzania RabbitMQ: http://localhost:15672 (Domyślny Login: guest, Hasło: guest).

7. Rozwiązywanie problemów (Troubleshooting)
Błąd: "Connection refused" lub "Network-related error" przy Update-Database

    Przyczyna: Kontener z SQL Serverem nie jest uruchomiony.

    Rozwiązanie: Otwórz terminal w folderze projektu i wpisz docker-compose up -d. Upewnij się w Docker Desktop, że kontenery świecą się na zielono.

Błąd: "MassTransit License Exception"

    Przyczyna: Zainstalowana wersja biblioteki MassTransit (v8.2+) wymaga konfiguracji licencji.

    Rozwiązanie: W Menedżerze pakietów NuGet dla rozwiązania zmień wersję pakietów MassTransit oraz MassTransit.RabbitMQ na starszą, stabilną wersję 8.1.3.

Błąd: Brak danych na liście zasobów w UI

    Przyczyna: Brak połączenia z API lub użytkownik nie jest zalogowany.

    Rozwiązanie: Zaloguj się w aplikacji UI. Sprawdź, czy projekt API jest uruchomiony.