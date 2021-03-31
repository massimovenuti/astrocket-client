using UnityEngine;

[System.Serializable]
class Keys : ISaveItem
{
    public InputKey[] keys;

    public Keys(params InputKey[] keys)
    {
        this.keys = keys ?? new InputKey[] 
        { 
            new InputKey("aze", KeyCode.Mouse0), 
            new InputKey("test", KeyCode.Space)
        };
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
