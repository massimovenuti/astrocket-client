using UnityEngine;

[System.Serializable]
class VideoSettings
{
    public Vector2 Resolution;
    public float AspectRation;
    public AAMode AntiAliasing;
}

public enum AAMode
{
    SMAA,
    FXAA,
    None
}
