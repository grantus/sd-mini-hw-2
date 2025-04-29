## Какие пункты из требуемого функционала вы реализовали и в каких классах модулях их можно увидеть

1. Добавить / удалить животное

   `AnimalsController.Add`
   `AnimalsController.RemoveAnimal`

2. Добавить / удалить вольер
   
   `EnclosuresController.Add/RemoveEnclosure`

3. Переместить животное между вольерами
   
   `AnimalTransferService.Move` + `ZooApi.PatchMoveAnimal`

4. Просмотреть расписание кормления

   `FeedingController`

5. Добавить новое кормление в расписание 

   `FeedingController`

6. Просмотреть статистику зоопарка (кол-во животных, свободные вольеры и т.д.).

   `GetNumOfFreeCages()` (class `Enclosures`), `Zoo.GetNumOfAnimals()` (class `Zoo`)

## Принципы Clean Architecture, примененные в проекте

Слои:
1. Presenter
   - «Контроллеры» `AnimalsController`, `EnclosuresController`, `FeedingController`, `StatisticsController` (`Application.cs`)
   - Обёртка-фасад `ZooApi`
   - Точка входа `Program.cs`
2. Domain

    Классы: `Animal`, `Herbo`, `Predator`, `Monkey`, `Rabbit`, `Tiger`, `Wolf`, `Enclosure`, `Thing`, `FeedingSchedule`, `FeedingSchedule.FoodTypes`, делегат `TotalFoodChanged`
   
    Их роли - чистая предметная модель, бизнес-правила, сущности, value-objects, события
3. Data

   `InMemoryZooRepository`, `IVeterinaryClinic.VeterinaryClinic`
   - Хранит коллекции в памяти, стало быть, можно заменить на БД, не трогая `Application`.
   - Ветклиника демонстрирует работу с внешним сервисом.

## Концепции Domain-Driven Design
1. Entity

   - `Animal`, `Enclosure`, `FeedingSchedule` - объекты, чья идентичность важнее состояния.
   - `Animal.Id` и `Enclosure.Id` генерируются один раз и далее неизменяемы.
   - `FeedingSchedule` идентифицируется составным ключом (`Animal`, `Time`) — пара «кого кормим + когда» служит уникальным идентификатором.

2. Value Object
  
   `FeedingSchedule.FoodTypes` – неизменяемая оболочка над строкой-названием корма. Равенство определяется через значение `Name`; собственных идентификаторов нет.
  
3. Aggregate

   - Агрегат «Животное» – корень `Animal`, внутри может содержаться внутреннее состояние (корм, здоровье).
   - Агрегат «Вольер» – корень `Enclosure`; инвариант «не больше `Capacity` обитателей» соблюдается через сервис `AnimalTransferService`.
   - Агрегат «Расписание кормления» – корень `FeedingSchedule`. Ссылки между агрегатами — только по идентификаторам (`Animal` помещает вольер ссылкой на root, расписание хранит ссылку на `Animal`).

4. Domain Event
   
   `AnimalMovedEvent`, `FeedingTimeEvent` + `DomainEvents`. Корни агрегатов публикуют события, подписчики реагируют на них.  

5. Domain Service

   `AnimalTransferService` (перемещение между вольерами, проверка вместимости) и `FeedingOrganizationService` (учёт кормлений). Эти операции не привязаны к конкретной сущности, поэтому вынесены в отдельные сервисы.

6. Repository

   `InMemoryZooRepository` – абстракция коллекций агрегат-рутов (`Animals`, `Enclosures`, `FeedingSchedules`). Доступ к данным инкапсулирован; остальной код не знает, где и как они хранятся.
