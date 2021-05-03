using UnityEngine;

public class FirstScreenManager : ScreenManager
{
    public override void Start()
    {
#if UNITY_ANDROID
        Screen.orientation = ScreenOrientation.Portrait;
#endif
        base.Start();
    }

    void Update()
    {
        if (Input.anyKeyDown)
            goToNextPage();
    }
}
