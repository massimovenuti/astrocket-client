using UnityEngine;
using System.Linq;

public class OnMapExit : MonoBehaviour
{
    private ReturnToBattle _rtb;

    private void Awake( )
    {
        _rtb = GameObject.FindGameObjectsWithTag("MainCamera").First().transform.parent.GetComponent<ReturnToBattle>();
        
        if(_rtb == null)
            Debug.LogError($"There were no GameObjects with tag MainCamera nor having the component ReturnToBattle");
    }

    private void OnTriggerEnter(Collider intruder)
    {
        if (this.CompareTag("WarningBorder") && intruder.CompareTag("Player"))
        {
            _rtb.EnterArea();
        }
        else if (this.CompareTag("SmoothBorder") && intruder.CompareTag("Asteroid"))
        {
            intruder.GetComponent<Asteroid>().inMapBounds = true;
        }
    }

    private void OnTriggerExit(Collider intruder)
    {
        if (this.CompareTag("WarningBorder"))
        {
            if (intruder.CompareTag("Player"))
            {
                _rtb.ExitArea();
            }
        }
        else if(this.CompareTag("SmoothBorder"))
        {
            if (intruder.CompareTag("Asteroid"))
            {
                intruder.GetComponent<Asteroid>().inMapBounds = false;
            }
        }
        else if(this.CompareTag("AbsoluteBorder"))
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
}
