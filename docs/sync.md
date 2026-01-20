# Moduł Synchronizacji (Worker Service)

Usługa odpowiedzialna za spójność danych w systemie rozproszonym.

## Technologie
* **MassTransit:** Abstrakcja nad kolejką wiadomości.
* **RabbitMQ:** Broker wiadomości.

## Przepływ danych (Event Driven)
1. W module API powstaje obiekt `Resource`.
2. MassTransit publikuje zdarzenie `ResourceCreated` (zdefiniowane w `Core`).
3. Wiadomość trafia do exchange w RabbitMQ.
4. `Synchronizacja` posiada zarejestrowany `ResourceCreatedConsumer`.
5. Consumer odbiera wiadomość i sprawdza, czy dany zasób istnieje w lokalnej bazie.
6. Jeśli nie – następuje zapis (replikacja).

## Obsługa błędów
W przypadku niedostępności RabbitMQ, usługa automatycznie ponawia próbę połączenia dzięki wbudowanym mechanizmom MassTransit (Retry Policy).