class Actions
{
    private static List<Action> actions;
    private static string userMenu = "";

    private static void setUserMenu()
    {
        if (userMenu.Length > 0)
        {
            return;
        }

        for (int i = 0; i < actions.Count; i++)
        {
            userMenu += $"{i}. {actions[i].name}\n";
        }

        userMenu += $"\n\n-1. Сохранить и выйти";
    }

    private static bool parseComplicatedQuery(string query, Player player, string effectKey)
    {
        // чекайте конфиг, чтобы понять, что такое "сложный запрос". Сплитим запрос по символам.
        // то есть условие вида "10;60:мана>40&мана<70" ознаает, что в случае выполнения условий после двоеточия
        // применится правый параметр (+60), в случае невыполнения - левый
        string[] splittedQuery = query.Split(new[] { ';', ':', '&' }, StringSplitOptions.RemoveEmptyEntries);
        int leftNumber = int.Parse(splittedQuery[0]);
        int rightNumber = int.Parse(splittedQuery[1]);

        // счетчик для выполненных условий
        int trueConditionsCounter = 0;

        bool toReturn = false;

        // бьем по знакам неравенства, берем значение, чтобы сравнить текущий параметр с тем, что задан в конфиге
        for (int i = 2; i < splittedQuery.Length; i++)
        {
            if (splittedQuery[i].IndexOf('>') != -1)
            {
                string[] splittedCondition = splittedQuery[i].Split(new[] { '>' }, StringSplitOptions.RemoveEmptyEntries);
                if (player.getParameter(splittedCondition[0]) > int.Parse(splittedCondition[1]))
                {
                    trueConditionsCounter++;
                }
            }

            if (splittedQuery[i].IndexOf('<') != -1)
            {
                string[] splittedCondition = splittedQuery[i].Split(new[] { '<' }, StringSplitOptions.RemoveEmptyEntries);
                if (player.getParameter(splittedCondition[0]) < int.Parse(splittedCondition[1]))
                {
                    trueConditionsCounter++;
                }
            }

            // Если условия выполнены, то все ок, если нет, то гг
            if (trueConditionsCounter == splittedQuery.Length - 2)
            {
                toReturn = player.setParameter(effectKey, player.getParameter(effectKey) + rightNumber);
            }
            else
            {
                toReturn = player.setParameter(effectKey, player.getParameter(effectKey) + leftNumber);
            }

        }
        return toReturn;
    }

    public static string getUserMenu()
    {
        return userMenu;
    }

    public static void setActions(List<Action> actionList)
    {
        actions = actionList;
        setUserMenu();
    }

    public static bool doAction(int actionIndex, Player player)
    {
        if (actionIndex > actions.Count)
        {
            Console.WriteLine("Действия с таким номером нет");
            return false;
        }

        Action action = actions[actionIndex];

        // Если для действия есть минимальные требования по каким-либо параметрам, то тупо сравниваем
        // текущие параметры с требуемыми
        if (action.requirements != null)
        {
            Dictionary<string, Requirement>.KeyCollection parametersKeys = action.requirements.Keys;
            foreach (string parameterKey in parametersKeys)
            {
                int currentParameterValue = player.getParameter(parameterKey);
                if (action.requirements[parameterKey].min != null && action.requirements[parameterKey].max != null)
                {
                    if (currentParameterValue > action.requirements[parameterKey].min &&
                        currentParameterValue < action.requirements[parameterKey].max)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (action.requirements[parameterKey].min != null)
                {
                    if (currentParameterValue > action.requirements[parameterKey].min)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (currentParameterValue < action.requirements[parameterKey].max)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        // Получаем значения эффектов (последствий этого действия, например, здоровье: +30)
        Dictionary<string, Effect>.KeyCollection effectsKeys = action.effects.Keys;
        foreach (string effectKey in effectsKeys)
        {
            // если в строке есть точка с запятой, то это сложный запрос и за его парсинг отвечает другой метод (выше описан)
            if (action.effects[effectKey].change.IndexOf(';') != -1)
            {
                bool parseComplicatedDoneSuccessfully = parseComplicatedQuery(action.effects[effectKey].change, player, effectKey);
                if (!parseComplicatedDoneSuccessfully)
                {
                    return false;
                }
            }
            else
            {
                // если это норм запрос, то просто создаем новое значение параметра (старое значение + эффект)
                // устанавливаем значение через метод класса Player, который возвращает bool.
                int newValueOfParameter = player.getParameter(effectKey) + int.Parse(action.effects[effectKey].change);
                bool settingValueSuccessfully = player.setParameter(effectKey, newValueOfParameter);
                if (!settingValueSuccessfully)
                {
                    return false;
                }
            }
        }

        return true;
    }
}