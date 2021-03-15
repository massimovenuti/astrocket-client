using UnityEngine;
using Mirror;

public class PlayerHealth : NetworkBehaviour
{
    public Health playerHealth;

    // Start is called before the first frame update
    private void Start( )
    {
        playerHealth = new Health(100);

        // DEBUG
        Debug.Log("Health : " + playerHealth.GetHealth());
    }

    // Fonction diminuant la vie du joueur lorsqu'il
    // est touché par quelque chose
    private void OnCollisionEnter(Collision collision)
    {
        if (!isLocalPlayer)
            return;

        GameObject go = collision.gameObject;

        if (go.tag == "Bullet" && go.GetComponent<Bullet>().ownerId != netId)
        {
            Debug.Log("(" + netId + ") Touched by a bullet from : " + go.GetComponent<Bullet>().ownerId);
            CmdDestroyObject(go);
            CmdDamage(20);
        }

        if (go.tag == "Asteroid")
        {
            Debug.Log("Touched by an asteroid");
            CmdDestroyObject(go);
            CmdDamage(25);
        }

        Debug.Log("Health : " + playerHealth.GetHealth());

        if (playerHealth.GetDead())
        {
            // DEBUG
            Debug.Log("It's dead :(");
            CmdRevive();
        }
    }

    [Command]
    private void CmdDamage(int damage)
    {
        RpcDamage(damage);
    }

    [Command]
    private void CmdRevive()
    {
        RpcRevive();
    }

    [TargetRpc]
    private void RpcDamage(int damage)
    {
        playerHealth.Damage(damage);
    }

    [TargetRpc]
    private void RpcRevive( )
    {
        // le joueur est mort, un script va le
        // désactiver pendant X secondes
        GameObject handler = GameObject.Find("Map");
        handler.SendMessage("SwitchPlayerActivation", gameObject);

        // Réinitialise la vie, et indique que
        // le joueur est à nouveau vivant
        playerHealth.SetDead(false);
        playerHealth.ResetHealth();

        // reset l'inertie
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    [Command]
    private void CmdDestroyObject(GameObject go)
    {
        NetworkServer.Destroy(go);
    }
}
