using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class PlayerDrone : NetworkBehaviour
{
    public GameObject drone;
    public GameObject bullet;
    public Material bulletMaterial;
    public PlayerHealth accessShield;

    public string BulletStorageTagName = "BulletStorage";

    public bool hasDrone;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    private void Start( )
    {
        if (isServer)
        {
            hasDrone = false;
        }

        // on désactive le drone au tout début
        drone = GameObject.Find("Drone");
        drone.SetActive(false);

        // on récupère le joueur auquel appartient le drone
        //_player = this.gameObject;

        // pour accéder au bouclier du joueur
        accessShield = this.GetComponent<PlayerHealth>();
    }

    /// <summary>
    /// Fonction activant le drone du joueur lorsqu'il
    /// récupère le power-up correspondant
    /// </summary>
    [Server]
    public void PowerUpDrone()
    {
        if (accessShield.hasShield)
        {
            accessShield.DesactivateShield();
        }

        hasDrone = true;
        drone.SetActive(true);
        RpcActiveDrone(true);
        StartCoroutine(TimerDrone());
    }

    /// <summary>
    /// Fonction attendant 30 secondes avant de
    /// désactiver le drone
    /// </summary>
    private IEnumerator TimerDrone( )
    {
        // TODO: change value
        yield return new WaitForSeconds(30);

        RpcActiveDrone(false);
        drone.SetActive(false);
        hasDrone = false;
    }

    /// <summary>
    /// Fonction désactivant le drone du joueur
    /// </summary>
    public void DesactivateDrone( )
    {
        RpcActiveDrone(false);
        drone.SetActive(false);
        hasDrone = false;
    }

    [ClientRpc]
    private void RpcActiveDrone(bool active)
    {
        drone.SetActive(active);
    }
}
