using UnityEngine;

[System.Serializable]
public class Sound : ISaveItem
{
    private static Sound SoundInstance = null;
    public static Sound Instance(float master = 1f, float music = 1f, float vfx = 1f)
    {
        if(SoundInstance == null)
            SoundInstance = new Sound(master, music, vfx);
        return SoundInstance;
    }

    public float master;
    public float effects;
    public float music;

    public float Master
    {
        get => master;
        set {
            GameObject[] gos = GameObject.FindGameObjectsWithTag("SoundMusic");
            foreach (GameObject go in gos)
                go.GetComponent<AudioSource>().volume = value* Music;
            gos = GameObject.FindGameObjectsWithTag("SoundFX");
            foreach (GameObject go in gos)
                go.GetComponent<AudioSource>().volume = value * Effects;
            master = value;
        }
    }
    public float Effects
    {
        get => effects;
        set {
            GameObject[] gos = GameObject.FindGameObjectsWithTag("SoundFX");
            foreach (GameObject go in gos)
                go.GetComponent<AudioSource>().volume = value * Effects;
            effects = value;
        }
    }

    public float Music
    {
        get => music;
        set {
            GameObject[] gos = GameObject.FindGameObjectsWithTag("SoundMusic");
            foreach(GameObject go in gos)
                go.GetComponent<AudioSource>().volume *= value;
            music = value;
        }
    }

    private Sound(float master = 1f, float effects = 1f, float music = 1f)
    {
        Master = master;
        Effects = effects;
        Music = music;
    }

    public string toSaveItem()
    {
        return JsonUtility.ToJson(this);
    }
}
