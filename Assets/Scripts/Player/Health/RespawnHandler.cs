using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnHandler : MonoBehaviour
{
    // Fonction désactivant le joueur et lançant
    // le compte à rebours avant la réactivation
    void SwitchPlayerActivation(GameObject Player)
    {
        Player.SetActive(false);
        StartCoroutine(WaitForReactivation(Player));
    }

    // Fonction attendant 2 secondes avant de
    // réactiver le joueur
    IEnumerator WaitForReactivation(GameObject Player)
    {
        // TODO: change value
        yield return new WaitForSeconds(2);
        Player.SetActive(true);
    }
}
