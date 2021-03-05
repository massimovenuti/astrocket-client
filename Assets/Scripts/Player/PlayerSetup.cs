using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    public Behaviour[] toDisable;
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
}