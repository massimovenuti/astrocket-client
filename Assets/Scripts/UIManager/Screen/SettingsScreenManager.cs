using UnityEngine;

public class SettingsScreenManager : ScreenManager
{
    void Start( )
    {
#if UNITY_ANDROID
        Screen.orientation = ScreenOrientation.Landscape;
#endif
        base.Start();
    }

    void Update( )
    {

    }
}
