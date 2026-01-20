# Interfejs Użytkownika (Blazor Server)

Aplikacja kliencka umożliwiająca obsługę systemu przez przeglądarkę.

## Struktura
* **Pages/Login.razor:** Formularz logowania. Zapisuje token JWT w pamięci sesji `AuthService`.
* **Pages/Register.razor:** Formularz rejestracji użytkownika.
* **Pages/Resources.razor:** Główny widok listy zasobów.
    * Pobiera początkowe dane przez HTTP GET.
    * Utrzymuje otwarte połączenie WebSocket (SignalR).
    * Gdy przyjdzie zdarzenie `ReceiveResource`, lista jest aktualizowana dynamicznie (bez przeładowania strony).

## Serwisy
* **AuthService:** Odpowiada za komunikację z endpointami autoryzacji oraz przechowywanie tokena JWT.