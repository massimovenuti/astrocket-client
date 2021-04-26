using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

[Serializable]
static class GeneralSettings
{
    public static Keys keys;
    public static Sound sound;

    private static List<ISaveItem> settings;

    public static void SetSettings(GeneralSettingsProxy gsp)
    {
        SetSettings(gsp.sound);
        SetSettings(gsp.keys);
    }
    public static void SetSettings(Sound s) => sound = s; 
    public static void SetSettings(Keys ks) => keys = ks; 
    /// <summary>
    /// Get the JSON representation for every member of the class
    /// </summary>
    /// <returns>Json string (not prettified) representing the settings</returns>
    public static string toSaveItem( )
    {
        string json = "{\"settings\":{";

        IEnumerable<ISaveItem> s = typeof(GeneralSettings).GetFields().Where(field => typeof(ISaveItem).IsAssignableFrom(field.FieldType)).Select(f => (ISaveItem)f.GetValue(null));
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