using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
   [SerializeField] Behaviour[] toDisable;

    public override void OnStartClient( )
    {
        if (!isLocalPlayer)
        {
            foreach (Behaviour obj in toDisable)
            {
                obj.enabled = false;
            }
        }
        else
        {
            /*
            ResolutionManager sm = ResolutionManager.Instance;
            sm.SetResolution(sm.WindowedResolutions.Count - 1, false);
            */
            #if UNITY_ANDROID
                Transform t = transform.Find("Canvas/MobileControls");
                Debug.Log(t);
                t.gameObject.SetActive(true);
                InputManager.InputManagerInst.RegisterMobileUser(t.gameObject);
            #endif

        }
        GameObject.Find("ScoreCanvas").GetComponent<ScoreTabManager>().addLigne(gameObject);
    }
}