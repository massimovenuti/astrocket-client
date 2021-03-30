using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RocketExplosion : NetworkBehaviour
{
    [SerializeField] float radius = 25.0f;
    [SerializeField] float power = 100.0f;
    [SerializeField] GameObject ExplosionVFX;

    private Vector3 explosionPosition;

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Rocket") && other.GetComponent<Ammo>().ownerId != netId)
    //    {
    //        Explosion();
    //    }
    //}

    private void Explosion()
    {
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
            //else if (hit.CompareTag("Player") && hit.GetComponent<NetworkIdentity>().netId != _ownerId)
            //{
            //    hit.gameObject.GetComponent<PlayerHealth>().ExplosionDamage();
            //}
        }
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
