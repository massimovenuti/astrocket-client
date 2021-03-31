using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Rocket : NetworkBehaviour
{
    public float radius = 25.0f;
    public float power = 100.0f;
    public Vector3 explosionPosition;

    public GameObject BulletStorage;
    public GameObject PlayerParent;
    public GameObject RealPlayer;
    public GameObject ExplosionVFX;

    private uint _ownerId;

    public override void OnStartServer()
    {
        this._ownerId = GetComponent<Ammo>().ownerId;
    }

    /// <summary>
    /// Fonction gérant l'effet d'explosion et infligeant des
    /// dégâts quand la roquette touche quelque chose
    /// </summary>
    [ServerCallback]
    public void OnTriggerEnter(Collider collider)
    {

        if (collider.CompareTag("Player") && collider.GetComponent<NetworkIdentity>().netId == _ownerId)
        {
            return;
        }
        else if (!collider.CompareTag("Player") && !collider.CompareTag("Asteroid"))
        {
            return;
        }

        explosionPosition = gameObject.transform.position;

        RpcExplosion(explosionPosition);

        // on récupère tous les objets dans le rayon de l'explosion
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius);

        // pour chacun de ces objets, si c'est un joueur ou un astéroïde, 
        // on lui ajoute une force d'explosion et on lui inflige des 
        // dégâts (joueur) ou on le détruit (astéroïde)
        foreach (Collider hit in colliders)
        {
            if (hit.CompareTag("Asteroid"))
            {
                GetComponent<DestroyAsteroid>().AsteroidDestruction(hit.gameObject);
            }
            else if (hit.CompareTag("Player") && hit.GetComponent<NetworkIdentity>().netId != _ownerId)
            {
                hit.gameObject.GetComponent<PlayerHealth>().ExplosionDamage();
            }
        }

        NetworkServer.Destroy(this.gameObject);
    }

    [ClientRpc]
    private void RpcExplosion(Vector3 explosionPosition)
    {
        // on active l'effet de particule pour l'explosion
        // et on le désactive une fois fini
        GameObject explosion = Instantiate(ExplosionVFX, explosionPosition, Quaternion.identity);
        ParticleSystem explosionParticles = explosion.transform.GetChild(0).GetComponent<ParticleSystem>();
        explosionParticles.Play();
        float explositionDuration = explosionParticles.main.duration + explosionParticles.main.startLifetimeMultiplier;
        Destroy(explosion, explositionDuration);
    }
}
