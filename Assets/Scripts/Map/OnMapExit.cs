using UnityEngine;
using System.Linq;

public class OnMapExit : MonoBehaviour
{
    private ReturnToBattle _rtb;

    private void Awake( )
    {
        _rtb = GameObject.FindGameObjectsWithTag("MainCamera").First().transform.parent.GetComponent<ReturnToBattle>();
    }

    private void OnTriggerEnter(Collider intruder)
    {
        if (intruder.CompareTag("Player"))
            _rtb.EnterArea();
    }

    private void OnTriggerExit(Collider intruder)
    {
        if (intruder.CompareTag("Player"))
            _rtb.ExitArea();
    }
}
