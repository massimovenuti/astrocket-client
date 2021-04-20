using System;
using UnityEngine;

public class InputKey : ISaveItem
{
    public string keyname;
    public KeyCode key;

    public InputKey (string keyname, KeyCode key)
    {
        this.keyname = keyname;
        this.key = key;
    }
    
    public string toSaveItem()
    {
        return JsonUtility.ToJson(this, true);
    }

}
