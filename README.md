# SimbirGO
🚕🛴Vehicle rental service

# Задания для олимпиады Volga IT.

## Содержание

- [Задание](#задание)
  - [Требования](#требования)
- [Технологии](#технологии)
  - [Как запустить](#запуск)
- [Секреты](#секреты)
 
  
## Задание

Разработка сервиса по аренде автомобилей под названием “Simbir.GO”. [Sibmir GO](clck.ru/36J6AV).


## Требования
Разработка приложения
Необходимо реализовать данные контроллеры:
1. Контроллер аккаунтов / Админ контроллер аккаунтов
2. Контроллер оплаты
3. Контроллер транспорта / Админ контроллер транспорта
4. Контроллер аренды / Админ контроллер аренды


## Технологии 

* [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core)
* [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
* [Mapster](https://github.com/MapsterMapper/Mapster)
* [PostgreSQL](https://www.postgresql.org/)
  

#### Запуск
1. Поменяйте строку подключения к бд
 "ConnectionStrings": {
    "Postgres": "Host=localhost;Port=5432;Database=simbir-go-db;Username=postgres;Password=password"
  },
2.  Запустите проект
3.  Swagger URL: https://localhost:7151/swagger/index.html

#### Секреты для админа
1. {
    "Username": "Admin",
    "Password": "secret"
  },
