using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

[Serializable]
class GeneralSettings
{
    public Keys keys;
    public Sound sound;

    public GeneralSettings( )
    {
        keys = GameObject.FindObjectOfType<InputManager>().SaveInputs();
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