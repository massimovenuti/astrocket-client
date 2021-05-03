using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScreenManager : MonoBehaviour
{
    void Start()
    {
#if UNITY_ANDROID
        Screen.orientation = ScreenOrientation.Landscape;
#endif
    }

    void Update()
    {
        
    }
}
