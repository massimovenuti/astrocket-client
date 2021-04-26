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

    private void Start( )
    {
        if (!isLocalPlayer)
        {
            foreach (Behaviour obj in toDisable)
            {
                obj.enabled = false;
            }

            foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (p.GetComponent<NetworkBehaviour>().isLocalPlayer)
                {
                    p.GetComponentInChildren<ScoreTabManager>().addLigne(gameObject);
                }
            }
        }
        else
        {
            GameObject canvas = transform.Find("ScoreCanvas").gameObject;

            foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (p.GetComponent<NetworkBehaviour>().isLocalPlayer)
                {
                    canvas.GetComponent<ScoreTabManager>().addLigne(p);
                }
            }

#if UNITY_ANDROID
            Transform t = this.transform.Find("Canvas/MobileControls");
            t.gameObject.SetActive(true);
            InputManager.InputManagerInst.RegisterMobileUser(t.gameObject);
#endif
        }
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