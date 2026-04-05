# New_Web_Library

# Library & Forum Management System

## Description

Web application for managing a library system with an integrated forum module.

The project is based on a previous library management system (Web Library), which has been extended with a forum module. The new functionality allows users to share opinions, discuss books, and interact through posts and comments.

It demonstrates a layered architecture built with ASP.NET Core MVC and Entity Framework Core, following best practices for separation of concerns and maintainable code structure.
---

## Features

- Browse and explore available books
- View detailed information about books
- Create, edit and manage forum posts
- Comment on posts and participate in discussions
- User authentication and authorization
- Admin area for managing content
- Soft and hard delete functionality with restore options
- Service layer abstraction for handling business logic

---

## User Roles & Permissions

The application supports role-based access control:

### Guest (unauthenticated users)
- Can browse books
- Can view posts and comments
- Cannot create or modify content

### User (authenticated users)
- Can browse and explore available books
- Can borrow and reserve books
- Can create posts
- Can comment on posts
- Can edit and delete their own content
- Can submit complaints to the system administrator when necessary

### Admin
- Full access to all features
- Can register and manage users
- Can view posts, feedback, recommendations and complaints
- Can take appropriate actions based on user reports
- Can edit and delete any post or comment
- Can manage books and categories
- Can restore or permanently delete content

---

## Architecture

The application follows a layered (multi-tier) architecture designed to ensure separation of concerns, maintainability and scalability.

### Admin Area
- Separate area dedicated to administrative functionality
- Accessible only by users with Admin role
- Provides management of books, categories and system data
- Ensures separation between public and administrative features

### Web Layer (Presentation Layer)
- Contains controllers and views
- Handles HTTP requests and responses
- Responsible for user interaction and input validation (ModelState)

### Service Layer (Business Logic)
- Contains the core business logic of the application
- Processes data and applies validation rules
- Uses a ServiceResult pattern to return consistent responses (Success, ErrorMessage, Data)
- Acts as a bridge between the Web layer and the Data layer

### Data Layer (Repository Layer)
- Responsible for data access and communication with the database
- Implements repository pattern
- Uses Entity Framework Core for ORM operations

### Testing Layer
- Unit tests are implemented for the Service layer
- NUnit is used as the testing framework
- Moq is used for mocking dependencies
- Tests cover success, failure and exception scenarios
- Achieved approximately 70% code coverage in the service layer

### Authentication & Authorization
- The application uses ASP.NET Core Identity for user registration and authentication
- Supports role-based authorization (User / Admin)
- Provides secure login, registration and access control

---

## Configuration


A default system administrator is automatically created on first run using predefined credentials configured in the application.

For security reasons, sensitive data such as passwords should be changed after the initial setup.


---

## Technologies

- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- NUnit (for unit testing)
- Moq (for mocking dependencies)

---

## How to Run

Before running the application:

1. Configure the connection string according to your local SQL Server settings.

2. Apply database migrations using:


3. Run the application.

On first run:
- a default system administrator is created
- initial (seed) data required for the forum is generated


---

## Usage

After running the application:

- Register a new account
- Log in to access full functionality
- Explore books and participate in discussions

---

## Описание

Уеб приложение за управление на библиотечна система с интегриран форумен модул.

Проектът е базиран на предишна система за управление на библиотека (Web Library), която е разширена с форумен модул. Новата функционалност позволява на потребителите да споделят мнения, да обсъждат книги и да взаимодействат чрез публикации и коментари.

Приложението използва слоеста архитектура, изградена с ASP.NET Core MVC и Entity Framework Core, следвайки добри практики за разделяне на отговорностите и поддържаема структура на кода.
---
## Функционалности

- Преглеждане и разглеждане на наличните книги
- Преглед на детайлна информация за книги
- Създаване, редактиране и управление на публикации във форума
- Коментиране и участие в дискусии
- Аутентикация и оторизация на потребители
- Административен панел за управление на съдържанието
- Soft и Hard delete с възможност за възстановяване
- Сървис слой за обработка на бизнес логиката
---

## Роли и права на потребителите

Приложението използва ролево базиран достъп:

### Гост (нерегистриран потребител)
- Може да разглежда книги
- Може да вижда публикации и коментари
- Не може да създава или променя съдържание

### Потребител (регистриран)
- Може да разглежда наличните книги
- Може да заема и резервира книги
- Може да създава публикации
- Може да коментира
- Може да редактира и изтрива собственото си съдържание
- Може да изпраща сигнали към администратора при необходимост

### Администратор
- Има пълен достъп до всички функционалности
- Може да регистрира и управлява потребители
- Може да преглежда мнения, препоръки и сигнали
- Може да предприема действия спрямо подадените сигнали
- Може да редактира и изтрива всяка публикация или коментар
- Управлява книги и категории
- Може да възстановява или окончателно да изтрива съдържание


---

## Архитектура

Приложението използва многослойна (layered) архитектура, която осигурява разделяне на отговорностите, по-добра поддръжка и разширяемост.

### Административна зона (Admin Area)
- Отделна зона за административни функционалности
- Достъпна само за потребители с роля Admin
- Позволява управление на книги, категории и системни данни
- Осигурява разделение между публичната и административната част на приложението

### Уеб слой (Presentation Layer)
- Съдържа контролери и изгледи
- Обработва HTTP заявки и отговори
- Отговаря за взаимодействието с потребителя и валидацията на входните данни

### Сървис слой (Business Logic)
- Съдържа основната бизнес логика на приложението
- Обработва данни и прилага валидационни правила
- Използва ServiceResult модел за връщане на резултати (Success, ErrorMessage, Data)
- Свързва уеб слоя с data слоя

### Data слой (Repository Layer)
- Отговаря за достъпа до базата данни
- Използва repository pattern
- Използва Entity Framework Core за работа с базата данни

### Тестов слой
- Имплементирани са unit тестове за сървис слоя
- Използва се NUnit като тестова рамка
- Използва се Moq за mock-ване на зависимости
- Тестовете покриват успешни, неуспешни и exception сценарии
- Постигнато е около 70% покритие на кода в сървис слоя

### Аутентикация и оторизация
- Приложението използва ASP.NET Core Identity за регистрация и вход на потребители
- Поддържа ролево базиран достъп (User / Admin)
- Осигурява сигурен login и регистрация

---

## Конфигурация


При първото стартиране се създава системен администратор с предварително зададени настройки.

От съображения за сигурност е препоръчително тези данни да бъдат променени след първоначалното стартиране.

---

## Technologies

- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- NUnit (for unit testing)
- Moq (for mocking dependencies)

---

## Стартиране

Преди да стартирате приложението:

1. Конфигурирайте connection string-а спрямо настройките на вашия локален SQL Server.

2. Приложете миграциите към базата данни чрез командата:

3. Стартирайте приложението.

При първото стартиране:
- се създава системен администратор
- се добавя начална (seed) информация, необходима за работата на форума

---

## Използване

След стартиране на приложението:

- Регистрирайте нов потребител
- Влезте в системата
- Разглеждайте книги и участвайте в дискусии

---


