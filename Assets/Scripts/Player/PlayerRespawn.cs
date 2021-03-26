using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerRespawn : NetworkBehaviour
{
    [Server]
    public void Respawn( )
    {
        GetComponent<BoxCollider>().enabled = false;
        RpcActive(false);
        RpcRespawn();
        StartCoroutine(WaitForReactivation());
    }

    // Fonction attendant 2 secondes avant de réactiver le joueur
    [Server]
    private IEnumerator WaitForReactivation( )
    {
        yield return new WaitForSeconds(2);
        RpcActive(true);
        GetComponent<BoxCollider>().enabled = true;
    }

    [ClientRpc]
    private void RpcActive(bool active)
    {
        if (!isLocalPlayer && !isServer)
        {
            gameObject.SetActive(active);
        }
    }

    [TargetRpc]
    private void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            GameObject handler = GameObject.Find("Map");
            handler.SendMessage("SwitchPlayerActivation", gameObject);
        }
    }
}
