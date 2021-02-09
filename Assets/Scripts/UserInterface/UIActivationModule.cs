using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIActivationModule : MonoBehaviour
{
    public GameObject MobileUI;
    public GameObject DesktopUI;

    private void Awake( )
    {
#if UNITY_ANDROID
        DesktopUI.SetActive(false);                
#else     
        MobileUI.SetActive(false);
#endif 
    }

}
