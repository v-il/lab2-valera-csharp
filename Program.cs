class Program
{
    static void Main(string[] argc)
    {
        // Загружаем конфиг
        bool configLoadedSuccessfully = ConfigActions.LoadConfig();

        // Если конфиг не загрузился, загружаем по новой (после неудачной загрузки создался новый валидный конфиг)
        while (!configLoadedSuccessfully)
        {
            configLoadedSuccessfully = ConfigActions.LoadConfig();
        }

        // Получаем пункты меню (прописаны в конфиге)
        string menu = Actions.getUserMenu();

        // Получаем сейв, если есть
        List<int> savedGameValues = Saves.getSave();
        // Объявляем игрока
        Player player;

        // Если в массиве сейва нам пришли какие-то значения, то загружаем их, если нет, то ставим по умолчанию
        if (savedGameValues.Count > 0)
        {
            player = new Player(savedGameValues[0], savedGameValues[1], savedGameValues[2], savedGameValues[3], savedGameValues[4]);
        }
        else
        {
            player = new Player(100, 0, 10, 0, 0);
        }

        while (true)
        {
            // Выводим параметры игрока и меню
            Console.WriteLine(player.getParametersInOneLine());
            Console.WriteLine(menu);

            // считываем номер пункта действия 
            int actionId;
            if (int.TryParse(Console.ReadLine(), out actionId))
            {
                // Игра завершается, если пункт отрицательный
                if (actionId < 0)
                {
                    break;
                }
                // Передаем id действия в метод класса действий
                bool actionSuccessfully = Actions.doAction(actionId, player);
                if (!actionSuccessfully)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Это действие недоступно сейчас для вашего персонажа");
                    Console.ResetColor();
                }
                else
                {
                    Console.Clear();
                }
            }
            else
            {
                Console.WriteLine("Неверный формат числа. Введите целое число.");
            }
        }
        // Сохранение
        Console.WriteLine("Сохраняем...");
        Saves.addSave(player.getParametersAsArray());
        Console.WriteLine("Нажмите любую клавишу для закрытия консоли");

        Console.ReadKey();
    }
}