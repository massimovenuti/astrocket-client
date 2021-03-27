using UnityEngine;

public class StartScreenManager : ScreenManager
{
    void Start()
    {
#if UNITY_ANDROID
        Screen.orientation = ScreenOrientation.Portrait;
#endif
    }

    void Update()
    {
        
    }
}
