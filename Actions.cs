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
        string[] splittedQuery = query.Split(new[] { ';', ':', '&' }, StringSplitOptions.RemoveEmptyEntries);
        int leftNumber = int.Parse(splittedQuery[0]);
        int rightNumber = int.Parse(splittedQuery[1]);

        int trueConditionsCounter = 0;

        bool toReturn = false;

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
        Dictionary<string, Effect>.KeyCollection effectsKeys = action.effects.Keys;
        foreach (string effectKey in effectsKeys)
        {
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