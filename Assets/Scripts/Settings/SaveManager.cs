using System.IO;
using UnityEngine;

class SaveManager
{

    private static string _filename = @"/settings.json";

    public static void Save(GeneralSettings generalSettings)
    {
        string s = generalSettings.toSaveItem();
        string path = Application.persistentDataPath + _filename;

        if (File.Exists(path))
            File.WriteAllText(path, string.Empty);
        else
            File.Create(path).Dispose();

        File.WriteAllText(path, s);
    }

    public static GeneralSettings Load( )
    {
        string path = Application.persistentDataPath + _filename;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            GeneralSettings setting;
            try
            {
                setting = JsonUtility.FromJson<GeneralSettings>(json);
            }
            catch
            {
                setting = new GeneralSettings();
                Debug.LogError("Invalid JSON provided, getting new settings");
            }

            return setting;
        }
        else
        {
            return new GeneralSettings();
        }
    }
}
