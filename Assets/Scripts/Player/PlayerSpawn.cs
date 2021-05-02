using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSpawn : NetworkBehaviour
{
    // Protection au premier spawn
    public override void OnStartServer( )
    {
        Protect(true);
        GetComponent<GunController>().DisableShoot();

        StartCoroutine(ProtectedOnSpawn());
    }

    [ClientCallback]
    private void Start( )
    {
        OnEnable();
    }


    [Server]
    public void Respawn( )
    {
        Protect(true);

        // Réinitialise le joueur
        GetComponent<Movements>().RpcResetVelocity();
        GetComponent<PlayerHealth>().Revive();
        GetComponent<PlayerHealth>().DesactivateShield();
        GetComponent<GunController>().ResetShooting();

        // Désactive le joueur chez tous les clients
        RpcActive(false);

        StartCoroutine(WaitForReactivation());
    }

    [ClientCallback]
    private void OnEnable( )
    {
        if (isLocalPlayer)
        {
            // Indique au joueur de se TP à un point de spawn
            GameObject handler = GameObject.Find("Map");
            transform.position = handler.GetComponent<RespawnManager>().GetSafeRespawnPoint();
        }
    }

    // Fonction attendant 2 secondes avant de réactiver le joueur
    [Server]
    private IEnumerator WaitForReactivation()
    {
        // TODO: change value
        yield return new WaitForSeconds(2f);

        // Active le joueur chez tous les clients
        RpcActive(true);

        // Protection au spawn
        GetComponent<GunController>().DisableShoot();
        StartCoroutine(ProtectedOnSpawn());
    }

    [Server]
    private IEnumerator ProtectedOnSpawn( )
    {
        yield return new WaitForSeconds(2f);
        Protect(false);
    }

    [ClientRpc]
    private void RpcActive(bool active)
    {
        if (!isServer)
        {
            gameObject.SetActive(active);
        }
    }

    [Server]
    private void Protect(bool protection)
    {
        GetComponent<BoxCollider>().enabled = !protection;
    }
}
