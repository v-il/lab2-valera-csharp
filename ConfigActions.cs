using System.Text.Json;
using System.Collections.Generic;
using System.Text.Json.Nodes;

public class Effect
{
    public string change { get; set; }
}

public class Requirement
{
    public int? min { get; set; }
    public int? max { get; set; }
}

public class Action
{
    public string name { get; set; }
    public Dictionary<string, Requirement>? requirements { get; set; }
    public Dictionary<string, Effect> effects { get; set; }
}

public class ActionList
{
    public List<Action> actions { get; set; }
}

class ConfigActions
{
    private static void setDefaultConfig()
    {
        string oldConfigName = "config.json";
        string newConfigName = $"config_{DateTime.Now.ToString("yyyyMMddHHmmss")}.json";

        File.Move(oldConfigName, newConfigName);

        string jsonString = @"{
            ""actions"": [
                {
                    ""name"": ""Пойти на работу"",
                    ""requirements"": {
                        ""мана"": {""max"": 50},
                        ""усталость"": {""max"": 10}
                    },
                    ""effects"": {
                        ""жизнерадостность"": {""change"": ""-5""},
                        ""мана"": {""change"": ""-30""},
                        ""деньги"": {""change"": ""100""},
                        ""усталость"": {""change"": ""70""}
                    }
                },
                {
                    ""name"": ""Созерцать природу"",
                    ""effects"": {
                        ""жизнерадостность"": {""change"": ""1""},
                        ""мана"": {""change"": ""-10""},
                        ""усталость"": {""change"": ""10""}
                    }
                },
                {
                    ""name"": ""Пить вино и смотреть сериал"",
                    ""effects"": {
                        ""жизнерадостность"": {""change"": ""-1""},
                        ""мана"": {""change"": ""30""},
                        ""усталость"": {""change"": ""10""},
                        ""здоровье"": {""change"": ""-5""},
                        ""деньги"": {""change"": ""-20""}
                    }
                },
                {
                    ""name"": ""Сходить в бар"",
                    ""effects"": {
                        ""жизнерадостность"": {""change"": ""1""},
                        ""мана"": {""change"": ""60""},
                        ""усталость"": {""change"": ""40""},
                        ""здоровье"": {""change"": ""-10""},
                        ""деньги"": {""change"": ""-100""}
                    }
                },
                {
                    ""name"": ""Выпить с маргинальными личностями"",
                    ""effects"": {
                        ""жизнерадостность"": {""change"": ""5""},
                        ""мана"": {""change"": ""60""},
                        ""усталость"": {""change"": ""80""},
                        ""здоровье"": {""change"": ""-80""},
                        ""деньги"": {""change"": ""-150""}
                    }
                },
                {
                    ""name"": ""Петь в метро"",
                    ""effects"": {
                        ""деньги"": {""change"": ""10;60:мана>40&мана<70""},
                        ""мана"": {""change"": ""10""},
                        ""жизнерадостность"": {""change"": ""1""},
                        ""усталость"": {""change"": ""20""}
                    }
                },
                {
                    ""name"": ""Спать"",
                    ""effects"": {
                        ""здоровье"": {""change"": ""0;90:мана<30""},
                        ""жизнерадостность"": {""change"": ""0;-3:мана>70""},
                        ""мана"": {""change"": ""-50""},
                        ""усталость"": {""change"": ""-70""}
                    }
                }
            ]
        }";

        File.WriteAllText(oldConfigName, jsonString);
    }

    public static bool LoadConfig(string path = "config.json")
    {
        try
        {
            string configPath = path;
            if (!File.Exists(configPath))
            {
                Console.WriteLine("Конфиг не существует по этому пути. Взяты значения по умолчанию");
                configPath = "config.json";
            }

            string configContent = File.ReadAllText(configPath);

            ActionList json = JsonSerializer.Deserialize<ActionList>(configContent);

            List<Action> actionList = new List<Action>();

            foreach (var action in json.actions)
            {
                actionList.Add(action);
                if (action.effects == null || action.name == null)
                {
                    throw new Exception();
                } 
            }

            Actions.setActions(actionList);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("Некорректный конфиг, установлены значения по умолчанию");
            setDefaultConfig();
            return false;
        }
    }
}