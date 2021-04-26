using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
class Keys : ISaveItem
{
    public InputKey[] keys;

    private static Keys KeysInstance = null;
    public static Keys Instance(Dictionary<string, KeyCode> defaults = null)
    {
        if(KeysInstance == null)
            KeysInstance = new Keys(defaults ?? InputManager.DefaultKeys);
        return KeysInstance;
    }

    private Keys(Dictionary<string, KeyCode> defaults)
    {
        int i = 0;
        keys = new InputKey[defaults.Count];
        foreach (KeyValuePair<string,KeyCode> kp in defaults)
        {
            keys[i] = new InputKey(kp.Key, kp.Value);
            i++;
        }
    }

    public string toSaveItem()
    {
        string json = "[";
        foreach (InputKey k in keys)
            json += JsonUtility.ToJson(k) + ",";
        if (json.EndsWith(",")) // removes last comma
            json = json.Remove(json.Length - 1);
        json += "]";
        return json;
    }
}
