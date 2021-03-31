using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSpawn : NetworkBehaviour
{
    public override void OnStartServer( )
    {
        Protect(true);
        StartCoroutine(ProtectedOnSpawn());
    }

    [Server]
    public void Respawn( )
    {
        Protect(true);

        // Réinitialise le joueur
        GetComponent<Movements>().RpcResetVelocity();
        GetComponent<PlayerHealth>().Revive();
        GetComponent<PlayerHealth>().DesactivateShield();
        GetComponent<GunController>().ResetPowerUps();

        //this.GetComponent<Movements>().ResetPowerUps();
        //this.GetComponent<PlayerDrone>().DesactivateDrone();

        // Désactive le joueur chez tous les clients
        RpcActive(false);

        // Indique au joueur de respawn
        RpcRespawn();
    }

    [ClientRpc]
    private void RpcActive(bool active)
    {
        if (!isLocalPlayer && !isServer)
        {
            gameObject.SetActive(active);
        }
    }

    [ClientCallback]
    private void OnEnable( )
    {
        if (isLocalPlayer)
        {
            CmdRespawn();
        }
    }

    [Command]
    private void CmdRespawn()
    {
        RpcActive(true);
        StartCoroutine(ProtectedOnSpawn());
    }

    [Server]
    private IEnumerator ProtectedOnSpawn( )
    {
        yield return new WaitForSeconds(2f);
        Protect(false);
    }

    private void Protect(bool protection)
    {
        GetComponent<BoxCollider>().enabled = !protection;
        GetComponent<GunController>().canShoot = !protection;
    }

    [Server]
    private IEnumerator WaitForProtection( )
    {
        yield return new WaitForSeconds(2f);
        Protect(false);
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
