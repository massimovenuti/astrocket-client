using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    [SyncVar(hook = "OnColorChange")]
    public Color playerColor;

   [SerializeField] Behaviour[] toDisable;

    public override void OnStartClient( )
    {
        GameObject canvas = GameObject.Find("ScoreCanvas");

        if (!isLocalPlayer)
        {
            foreach (Behaviour obj in toDisable)
            {
                obj.enabled = false;
            }
        }
        else
        {

#if UNITY_ANDROID
            Transform t = this.transform.Find("Canvas/MobileControls");
            t.gameObject.SetActive(true);
            InputManager.InputManagerInst.RegisterMobileUser(t.gameObject);
#endif
        }

        canvas.GetComponent<ScoreTabManager>().addLigne(gameObject);
    }

    [ClientCallback]
    void OnColorChange(Color oldValue, Color newValue)
    {
        Renderer[] renders = GetComponentsInChildren<Renderer>();

        foreach (Renderer r in renders)
        {
            foreach (Material m in r.materials)
            {
                if (m.name.Contains("accent"))
                {
                    m.color = newValue;
                }
            }
        }
    }
}