# Dokumentacja API

Backend został zaimplementowany w ASP.NET Core Web API. Dokumentacja interaktywna dostępna jest pod adresem `/swagger` po uruchomieniu projektu.

## Bezpieczeństwo
API wykorzystuje tokeny **JWT (JSON Web Token)**.
* Token jest wydawany po poprawnym logowaniu (`POST /api/auth/login`).
* Dostęp do zasobów (`/api/resources`) wymaga nagłówka `Authorization: Bearer <token>`.

## Główne Endpointy

### Auth
| Metoda | Endpoint | Opis |
| --- | --- | --- |
| POST | `/api/auth/register` | Rejestracja nowego użytkownika. Hasła są haszowane (BCrypt). |
| POST | `/api/auth/login` | Logowanie. Zwraca token JWT. |

### Resources
| Metoda | Endpoint | Wymaga Auth? | Opis |
| --- | --- | --- | --- |
| GET | `/api/resources` | TAK | Pobiera listę wszystkich zasobów. |
| POST | `/api/resources` | TAK | Tworzy nowy zasób i inicjuje proces synchronizacji. |
| GET | `/api/resources/{id}` | TAK | Pobiera szczegóły zasobu. |

## SignalR Hub
Endpoint: `/notificationsHub`
* Metoda: `ReceiveResource` - wywoływana przez serwer w momencie dodania nowego zasobu. Przesyła obiekt `Resource` do wszystkich podłączonych klientów.