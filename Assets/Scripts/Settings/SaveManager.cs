using System.IO;
using UnityEngine;

class SaveManager : MonoBehaviour
{
    private void Awake( ) => Load();

    private static string _filename = @"/settings.json";

    public static void Save()
    {
        string s = GeneralSettings.toSaveItem();
        string path = Application.persistentDataPath + _filename;

        if (File.Exists(path))
            File.WriteAllText(path, string.Empty);
        else
            File.Create(path).Dispose();
        File.WriteAllText(path, s);
    }

    public static void Load( )
    {
        string path = Application.persistentDataPath + _filename;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            GeneralSettingsProxy settings;
            try
            {
                settings = JsonUtility.FromJson<GeneralSettingsProxy>(json);
                GeneralSettings.SetSettings(settings);
            }
            catch
            {
                GeneralSettings.keys = Keys.Instance();
                GeneralSettings.sound = Sound.Instance();
                Debug.LogError("Invalid JSON provided, getting new settings");
            }
        }
        else
        {
            GeneralSettings.SetSettings(Keys.Instance());
            GeneralSettings.SetSettings(Sound.Instance());
        }
    }
}
