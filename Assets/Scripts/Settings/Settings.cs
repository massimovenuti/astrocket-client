using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Settings
{
    public Audio audio;
    public Key[] keys;
    public Settings()
    {
        audio = new Audio();
        keys = new Key[InputManager.DefaultKeys.Count];
        int i = 0;
        foreach(var v in InputManager.DefaultKeys)
        {
            keys[i] = new Key();
            keys[i].key = v.Value;
            keys[i].keyname = v.Key;
            i++;
        }
    }
}

[System.Serializable]
public class Audio
{
    public float master;
    public float effects;
    public float music;
    
    public float Master
    {
        set
        {
            master = value;
            IEnumerable e = GameObject.FindGameObjectsWithTag("SoundFX");
            foreach (GameObject go in e)
            {
                go.GetComponent<AudioSource>().volume = (int)(value * (effects / 100));
            }
            e = GameObject.FindGameObjectsWithTag("SoundMusic");
            foreach (GameObject go in e)
            {
                go.GetComponent<AudioSource>().volume = (int)(value * (music / 100));
            }
        }

        get => master / 100;
    }

    public float Effects
    {
        set
        {
            effects = value;
        }

        get => effects * Master;
    }
    public float Music
    {
        set
        {
            music = value;
            IEnumerable e = GameObject.FindGameObjectsWithTag("SoundMusic");
            foreach (GameObject go in e)
            {
                go.GetComponent<AudioSource>().volume = (int)(master * (value / 100));
            }
        }

        get => music * Master;
    }

    public Audio(float master = 1f, float effects = 1f, float music = 1f)
    {
        this.music = music;
        this.master = master;
        this.effects = effects;
    }
}

[System.Serializable]
public class Key
{
    public KeyCode key;
    public string keyname;
}
