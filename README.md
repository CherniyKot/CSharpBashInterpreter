# CSharpBashInterpreter
Базовый интерпретатор командной строки, написанный на языке C#.

# Сборка проекта

Для работы над проектом необходимо использовать ![.NET 7](https://dotnet.microsoft.com/en-us/download/dotnet/7.0).
Проект не использует дополнительных внешних зависимостей, кроме подключаемых через nuget, поэтому для сборки проекта достаточно:
1) Перйти в папку проекта ```CSharpBashInterpreter``` 
2) Для сборки проекта вызывать ```dotnet build```
3) Для запуска проекта вызвать ```dotnet run```

# Структура программы

Парсер производит замену переменных окружения, разбиение строки на токены.  
После этого происходит матчинг полученных токенов с отсортированным по приоритету массивом мета-команд, после с массивом команд, после запрос к внешним программам  
При возможности парсинга команды (удачном вызове метода CanParse) происходит передача управления парсингом в соответствующую команду.  

Мета-команды формируют абстрактное синтаксическое дерево запроса, к таким командам относятся Pipe, Semicolon, If-Then-Else, etc.  
В листьях АСД находятся команды или запросы к внешним программам.  

После успешного парсинга происходит запуск команд АСД.

Каждая команда имеет 3 потока ввода-вывода, InputStream, OutputStream и ErrorStream. По умолчанию они заданны как соответствующие потоки консоли, но это поведение может быть перопределено мета-командами для перенаправления потоков в другие команды, файлы и т.д.  


## Схема классов:
![f drawio](https://user-images.githubusercontent.com/58166593/220185491-d5e84034-e4c6-4e36-9796-44d4b69aa409.png)
