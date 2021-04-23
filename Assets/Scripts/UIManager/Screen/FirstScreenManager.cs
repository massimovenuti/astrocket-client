using UnityEngine;

public class FirstScreenManager : ScreenManager
{
    new void Start()
    {
#if UNITY_ANDROID
#endif
        Screen.orientation = ScreenOrientation.Portrait;

        base.Start();
    }

    void Update()
    {
        if (Input.anyKeyDown)
            goToNextPage();
    }
}
