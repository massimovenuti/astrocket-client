using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsoluteBorder : MonoBehaviour
{
    private void OnTriggerExit(Collider intruder)
    {
        if (intruder.CompareTag("Player"))
        {
            PlayerHealth ph = intruder.GetComponent<PlayerHealth>();
            ph.playerHealth.Damage(ph.playerHealth.GetHealth());
        }
        else if(intruder.CompareTag("Asteroid"))
        {
            Destroy(intruder.gameObject);
        }
    }
}
