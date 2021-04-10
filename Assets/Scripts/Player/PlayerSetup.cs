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

    // Start is called before the first frame update
    void Start( )
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
            Button disconnectButton = transform.Find("MenuCanvas/DisconnectButton").GetComponent<Button>();
            disconnectButton.onClick.AddListener(GameObject.Find("NetworkManager").GetComponent<AsteroidNetworkManager>().StopClient);

            GameObject canvas = transform.Find("ScoreCanvas").gameObject;

            foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (p.GetComponent<NetworkBehaviour>().isLocalPlayer)
                {
                    canvas.GetComponent<ScoreTabManager>().addLigne(p);
                }
            }

#if UNITY_ANDROID
            InputManager.InputManagerInst.RegisterMobileUser(this.gameObject);
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