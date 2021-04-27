using System.IO;
using UnityEngine;

class SaveManager : MonoBehaviour
{
    private void Awake( ) => Load();

    private static string _filename = @"/settings.json";

    public static Settings Settings = new Settings();

    public static void Save()
    {
        string s = JsonUtility.ToJson(Settings);
        string path = Application.persistentDataPath + _filename;
        Debug.Log(s);
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
            try
            {
                Settings = JsonUtility.FromJson<Settings>(json);
                Debug.Log(Settings);
            }
            catch
            {
                Settings = new Settings(); 
                Debug.LogWarning("Invalid JSON provided, getting new settings");
            }
        }
    }
}
