using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    [SyncVar(hook = "OnColorChange")]
    public Color playerColor;

    [SerializeField] Behaviour[] toDisable;

    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer)
        {
            foreach (Behaviour obj in toDisable)
            {
                obj.enabled = false;
            }
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