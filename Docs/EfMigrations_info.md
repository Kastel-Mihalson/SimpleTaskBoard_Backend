# Памятка по использованию миграций Entity Framework Core
--------------------------------------------------------
Для миграций желательно использовать Package Manager Console.
*(For Visual Studio: Tools -> NuGet Package Manager -> Package Manager Console)*

## Преднастройка консоли:
-------------------------
В Default Project выбрать проект, в котором есть папка миграций и DbContext файл.
*(Для текущего проекта - это SimpleTaskBoard.Infrastructure)*

> Так как проект запускается через MultipleStartupProject, в который входят Content.API и Auth.API,
то в командах миграций нужно дописывать команду -StartupProject Content.API (либо -StartupProject Auth.API)

*[Для команд ниже также нужно указывать `-StartupProject Content.API`]*
1. Добавление миграций.
`Add-Migration SomeMigrationName`

2. Обновление БД.
`Update-Database`

3. Откат миграций из БД.
`Update-Database -Migration 0`

4. Удаление миграций
`Remove-Migration`
