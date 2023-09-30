class Program
{
    static void Main(string[] argc)
    {
        bool configLoadedSuccessfully = ConfigActions.LoadConfig();
        while (!configLoadedSuccessfully)
        {
            configLoadedSuccessfully = ConfigActions.LoadConfig();
        }
        string menu = Actions.getUserMenu();

        List<int> savedGameValues = Saves.getSave();
        Player player;
        
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
            Console.WriteLine(player.getParametersInOneLine());
            Console.WriteLine(menu);

            int actionId;
            if (int.TryParse(Console.ReadLine(), out actionId))
            {
                if (actionId < 0)
                {
                    break;
                }
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
        Console.WriteLine("Сохраняем...");
        Saves.addSave(player.getParametersAsArray());
        Console.WriteLine("Нажмите любую клавишу для закрытия консоли");

        Console.ReadKey();
    }
}