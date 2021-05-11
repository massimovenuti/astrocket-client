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
            ResolutionManager sm = ResolutionManager.Instance;
            sm.SetResolution(sm.WindowedResolutions.Count - 1, false);

            Transform t = this.transform.Find("Canvas/MobileControls");
            t.gameObject.SetActive(false);

            #if UNITY_ANDROID
                t.gameObject.SetActive(true);
                InputManager.InputManagerInst.RegisterMobileUser(t.gameObject);
            #endif
        }
        GameObject.Find("ScoreCanvas").GetComponent<ScoreTabManager>().addLigne(gameObject);
    }
}