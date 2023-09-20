# Добавление миграции
---------------------
- Открыть `Package manager console`
- В `default project` выбрать проект, в котором содержится DbContext (SimpleTaskBoard.Infrastructure)
- `StartUp Project` указать один из api проектов (Auth.API или Content.API)
- Выполнить команду `Add-Migration SomeMigrationName`

> Без указания старап проекта, вроде бы можно указать --startup-project Auth.API после команды Add-Migration <название миграции>

# Обновление БД
- `Update-Database`