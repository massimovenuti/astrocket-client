using System.Collections.Generic;
using System;
using System.Linq;

[Serializable]
class GeneralSettings
{
    public Keys keys;
    public Sound sound;

    public GeneralSettings( )
    {
        keys = new Keys(
            new InputKey("test", UnityEngine.KeyCode.None),
            new InputKey("test2", UnityEngine.KeyCode.Mouse0)
            );

        sound = new Sound();
    }

    private List<ISaveItem> settings;

    /// <summary>
    /// Get the JSON representation for every member of the class
    /// </summary>
    /// <returns>Json string (not prettified) representing the settings</returns>
    public string toSaveItem( )
    {
        string json = "{\"settings\":{";

        IEnumerable<ISaveItem> s = GetType().GetFields().Where(field => typeof(ISaveItem).IsAssignableFrom(field.FieldType)).Select(f => (ISaveItem)f.GetValue(this));
        foreach (ISaveItem item in s)
        {
            json += $"\"{item.GetType().Name}\":";
            json += item.toSaveItem();
            json += ",";
        }

        if (json.EndsWith(",")) // removes last comma
            json = json.Remove(json.Length - 1);

        json += "}}";
        return json;
    }
}