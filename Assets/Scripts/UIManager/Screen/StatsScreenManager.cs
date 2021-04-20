using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsScreenManager : ScreenManager
{
    void Start()
    {
#if UNITY_ANDROID
        Screen.orientation = ScreenOrientation.Portrait;
#endif
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
