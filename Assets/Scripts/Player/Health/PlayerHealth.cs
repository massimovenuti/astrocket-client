using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mirror;

public class PlayerHealth : NetworkBehaviour
{
    private Health _playerHealth;

    [SerializeField] int bulletDmg = 20;

    [SerializeField] int asteroidDmgRate = 10;

    [SerializeField] int maxPlayerHealth = 100;


    // Start is called before the first frame update
    private void Start( )
    {
        _playerHealth =  new Health(maxPlayerHealth);
    }

    // Fonction diminuant la vie du joueur lorsqu'il est touché par quelque chose
    private void OnTriggerEnter(Collider other)
    {
        if (!isLocalPlayer)
        {
            return;
        }

        GameObject go = other.gameObject;

        if (other.CompareTag("Bullet") && go.GetComponent<Bullet>().ownerId != netId)
        {
            CmdDestroyGo(go);
            _playerHealth.Damage(bulletDmg);
        }

        if (other.CompareTag("Asteroid"))
        {
            _playerHealth.Damage(go.GetComponent<Asteroid>().GetSize() * asteroidDmgRate);
            CmdDestroyGo(go);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        if (_playerHealth.IsDead())
        {
            Revive();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (other.CompareTag("AbsoluteBorder"))
        {
            Revive();
        }
    }

    public void Revive()
    {
        CmdActive(false);
        GameObject handler = GameObject.Find("Map");
        handler.SendMessage("SwitchPlayerActivation", gameObject);
    }


    private void OnEnable( )
    {
        if (!isLocalPlayer)
        {
            return;
        }
        
        // Reset l'inertie
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        // Réinitialise la vie, et indique que le joueur est à nouveau vivant
        _playerHealth.SetDead(false);
        _playerHealth.ResetHealth();

        CmdActive(true);
    }


    [Command]
    private void CmdActive(bool active)
    {
        RpcActive(active);
    }

    [ClientRpc]
    private void RpcActive(bool active)
    {
        if (!isLocalPlayer)
        {
            gameObject.SetActive(active);
        }
    }

    [Command]
    private void CmdDestroyGo(GameObject go)
    {
        NetworkServer.Destroy(go);
    }
}
