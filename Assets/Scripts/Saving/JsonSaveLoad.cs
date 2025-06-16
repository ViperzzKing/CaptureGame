using System.IO;
using UnityEngine;

public static class JsonSaveLoad
{
    //gives the folder of where app currently is
    public static string file = Application.dataPath + "/save.json";

    public static void Save(HighscoreData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(file, json);
    }

    public static HighscoreData Load()
    {
        if (File.Exists(file))
        {
            string json = File.ReadAllText(file);
            return JsonUtility.FromJson<HighscoreData>(json);
        }

        return null;
    }
}
