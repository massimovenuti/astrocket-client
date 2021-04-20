using UnityEngine;

[System.Serializable]
public class Sound : ISaveItem
{
    public float master;
    public float effects;
    public float music;

    public Sound(float master = 1f, float effects = 1f, float music = 1f)
    {
        this.master = master;
        this.effects = effects;
        this.music = music;
    }

    public string toSaveItem()
    {
        return JsonUtility.ToJson(this);
    }
}
