using UnityEngine;
using UnityEngine.UI;

public class PlayScreenManager : ScreenManager
{
    new void Start()
    {
#if UNITY_ANDROID
        Screen.orientation = ScreenOrientation.Portrait;
#endif
        base.Start();
    }

    void Update()
    {
        
    }
}
