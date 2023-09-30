public class Saves
{
    public static bool addSave(int[] parameters)
    {
        string fileName = "save.txt";
        try
        {
            string fileContent = string.Join(";", parameters);
            File.WriteAllText(fileName, fileContent);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Игра сохранена");
            Console.ResetColor();
            return true;
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("При сохранении произошла ошибка");
            Console.ResetColor();
            return false;
        }
    }

        public static List<int> getSave()
        {
            string fileName = "save.txt";
            List<int> parameters = new List<int>();

            if (File.Exists(fileName))
            {
                string fileContent = File.ReadAllText(fileName);
                string[] savedValues = fileContent.Split(new []{';'}, StringSplitOptions.RemoveEmptyEntries);

                if (savedValues.Length != 5)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Файл сохранения поврежден. Надо начать заново");
                    Console.ResetColor();
                    return new List<int>();
                }
                Dictionary<int, int[]> limitForIndex = new Dictionary<int, int[]>
                {
                    {0, new int[]{0, 100}},
                    {1, new int[]{0, 100}},
                    {2, new int[]{-10, 10}},
                    {3, new int[]{0, 100}}
                };
                
                for (int i = 0; i < 4; i++)
                {
                    if (int.Parse(savedValues[i]) >= limitForIndex[i][0] && int.Parse(savedValues[i]) <= limitForIndex[i][1])
                    {
                        parameters.Add(int.Parse(savedValues[i]));
                        continue;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Файл сохранения поврежден. Надо начать заново");
                        Console.ResetColor();
                        return new List<int>();
                    }
                }

                parameters.Add(int.Parse(savedValues[4]));
                
                while (true)
                {
                    Console.WriteLine("Обнаружен файл с сохранением. Загрузить?\n1 - да\n2 - нет");
                    ConsoleKeyInfo key = Console.ReadKey();
                    if (key.Key != ConsoleKey.D1 && key.Key != ConsoleKey.D2)
                    {
                        Console.Clear();
                        continue;
                    }

                    if (key.Key == ConsoleKey.D1)
                    {
                        Console.Clear();
                        return parameters;
                    }
                    else
                    {
                        Console.Clear();
                        return new List<int>();
                    }
                }
                
            }
            
            return parameters;
        }
}