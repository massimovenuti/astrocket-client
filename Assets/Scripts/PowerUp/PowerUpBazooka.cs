using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PowerUpBazooka : NetworkBehaviour
{
    [SerializeField] float destroyAfter = 5f;

    public override void OnStartServer( )
    {
        Invoke(nameof(DestroySelf), destroyAfter);
    }

    /// <summary>
    /// Quand le joueur entre dans la zone de trigger du power-up, appel de la fonction "PowerUpBazooka()" du script "GunController"
    /// et détruit le gameObject 
    /// </summary>
    [ServerCallback]
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.GetComponent<PlayerScore>().addPowerUP();
            collider.GetComponent<GunController>().PowerUpBazooka();
            NetworkServer.Destroy(this.gameObject);
        }
    }

    [Server]
    private void DestroySelf( )
    {
        NetworkServer.Destroy(gameObject);
    }
}
