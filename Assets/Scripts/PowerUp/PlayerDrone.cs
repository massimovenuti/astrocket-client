using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrone : MonoBehaviour
{
    public GameObject drone;
    public PlayerHealth accessShield;

    private GameObject _player;

    public bool hasDrone;

    // Start is called before the first frame update
    private void Start( )
    {
        drone = GameObject.Find("Drone");
        drone.SetActive(false);
        hasDrone = false;

        _player = this.gameObject.transform.GetChild(2).gameObject;

        accessShield = this.GetComponent<PlayerHealth>();
    }

    private void Update( )
    {
        drone.transform.RotateAround(_player.transform.position, Vector3.up, 80 * Time.deltaTime);
    }

    // Update is called once per frame
    public void PowerUpDrone()
    {
        // DEBUG
        Debug.Log("Drone");

        if (accessShield.hasShield)
            accessShield.DesactivateShield();

        hasDrone = true;
        drone.SetActive(true);
        StartCoroutine(TimerDrone());
    }

    // Fonction attendant 15 secondes avant de
    // désactiver le drone
    private IEnumerator TimerDrone( )
    {
        // TODO: change value
        yield return new WaitForSeconds(15);

        drone.SetActive(false);
        hasDrone = false;
    }

    public void DesactivateDrone( )
    {
        drone.SetActive(false);
        hasDrone = false;
    }
}
