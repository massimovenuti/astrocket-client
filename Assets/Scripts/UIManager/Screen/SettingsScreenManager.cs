using UnityEngine;

public class SettingsScreenManager : ScreenManager
{
    void Start( )
    {
#if UNITY_ANDROID
        Screen.orientation = ScreenOrientation.Portrait;
#endif
        base.Start();
    }

    void Update( )
    {

    }
}
