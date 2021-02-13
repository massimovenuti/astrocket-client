using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMapExit : MonoBehaviour
{
    private void OnTriggerEnter(Collider intruder) 
    {
        if(intruder.tag == "Player")
            ReturnToBattle.instance.EnterArea();
    }

    private void OnTriggerExit(Collider intruder) 
    {   
        if(intruder.tag == "Player")
            ReturnToBattle.instance.ExitArea();
    }
}
