using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PowerUpDrone : NetworkBehaviour
{
    [SerializeField] float destroyAfter = 5f;

    public override void OnStartServer( )
    {
        Invoke(nameof(DestroySelf), destroyAfter);
    }

    /// <summary>
    /// Quand le joueur entre dans la zone de trigger du power-up, appel de la fonction "PowerUpDrone()" du script "PlayerDrone"
    /// et détruit le gameObject 
    /// </summary>
    [ServerCallback]
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.GetComponent<PlayerDrone>().PowerUpDrone();
            NetworkServer.Destroy(this.gameObject);
        }
    }

    [Server]
    private void DestroySelf( )
    {
        NetworkServer.Destroy(gameObject);
    }
}
