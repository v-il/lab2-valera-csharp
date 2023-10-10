public class Player
{
    private Dictionary<string, int> parameters;

    public Player(int initHealth, int initMana, int initHappiness, int initFatigue, int initMoney)
    {
        parameters = new Dictionary<string, int>
        {
            {"здоровье", initHealth},
            {"мана", initMana},
            {"жизнерадостность", initHappiness},
            {"усталость", initFatigue},
            {"деньги", initMoney}
        };
    }

    public bool setParameter(string parameterKey, int parameterValue)
    {
        // Тупо валидация по границам, прописанным в лабе
        if ((parameterKey == "здоровье") &&
            parameterValue > 100)
        {
            parameterValue = 100;
        }

        if ((parameterKey == "здоровье") && parameterValue < 0)
        {
            return false;
        }

        if ((parameterKey == "мана" || parameterKey == "усталость") && parameterValue < 0)
        {
            parameterValue = 0;
        }

        if ((parameterKey == "мана" || parameterKey == "усталость") && parameterValue > 100)
        {
            return false;
        }

        if (parameterKey == "жизнерадостность" && parameterValue > 10)
        {
            parameterValue = 100;
        }

        if (parameterKey == "жизнерадостность" && parameterValue < -10)
        {
            return false;
        }

        if (parameterKey == "деньги" && parameters["деньги"] < 0 && parameterValue < parameters["деньги"])
        {
            return false;
        }

        parameters[parameterKey] = parameterValue;
        return true;
    }

    public int getParameter(string parameterKey)
    {
        return parameters[parameterKey];
    }

    public int[] getParametersAsArray()
    {
        return parameters.Values.ToArray();
    }

    public string getParametersInOneLine()
    {
        return $"Здоровье: {parameters["здоровье"]}\nМана: {parameters["мана"]}\nЖизнерадостность: {parameters["жизнерадостность"]}\nУсталость: {parameters["усталость"]}\nДеньги: {parameters["деньги"]}\n";
    }
}