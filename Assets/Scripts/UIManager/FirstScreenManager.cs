using UnityEngine;

public class FirstScreenManager : ScreenManager
{
    void Start()
    {
#if UNITY_ANDROID
        Screen.orientation = ScreenOrientation.Portrait;
#endif
    }

    void Update()
    {
        if (Input.anyKeyDown)
            goToNextPage();
    }
}
