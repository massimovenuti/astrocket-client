using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLifeTime : MonoBehaviour
{
    public float radius;
    public float power;
    public Vector3 explosionPosition;

    public GameObject BulletStorage;
    public GameObject PlayerParent;
    public GameObject RealPlayer;
    public GameObject ExplosionVFX;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        // TODO: change values
        radius = 25.0f;
        power = 100.0f;

        // on récupère le joueur ayant tiré la roquette
        BulletStorage = this.transform.parent.gameObject;
        PlayerParent = BulletStorage.gameObject.transform.parent.gameObject;
        RealPlayer = PlayerParent.gameObject.transform.Find("Player").gameObject;

        // la roquette a 1 seconde de durée de vie
        Destroy(gameObject, 1);
    }

    /// <summary>
    /// Fonction gérant l'effet d'explosion et infligeant des
    /// dégâts quand la roquette touche quelque chose
    /// </summary>
    public void OnCollisionEnter(Collision collider)
    {
        explosionPosition = gameObject.transform.position;

        // on active l'effet de particule pour l'explosion
        // et on le désactive une fois fini
        GameObject explosion = Instantiate(ExplosionVFX, explosionPosition, Quaternion.identity);
        ParticleSystem explosionParticles = explosion.transform.GetChild(0).GetComponent<ParticleSystem>();
        explosionParticles.Play();
        float explositionDuration = explosionParticles.main.duration + explosionParticles.main.startLifetimeMultiplier;
        Destroy(explosion, explositionDuration);

        // on récupère tous les objets dans le rayon de l'explosion
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius);

        // pour chacun de ces objets, si c'est un joueur ou un astéroïde, 
        // on lui ajoute une force d'explosion et on lui inflige des 
        // dégâts (joueur) ou on le détruit (astéroïde)
        foreach (Collider hit in colliders)
        {
            if (hit.tag == "Player" || hit.tag == "Asteroid")
            {
                if (hit.gameObject != RealPlayer)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();
                    if (rb != null)
                        rb.AddExplosionForce(power, explosionPosition, radius, 3.0f);

                    if (hit.tag == "Asteroid")
                        hit.gameObject.GetComponent<DestroyAsteroid>().DestructionAsteroid();

                    if (hit.tag == "Player")
                        hit.gameObject.GetComponent<PlayerHealth>().ExplosionDamage();
                }
            }
        }
    }
}
