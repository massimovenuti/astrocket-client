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

    // Start is called before the first frame update
    void Start()
    {
        // TODO: change values
        radius = 25.0f;
        power = 100.0f;

        BulletStorage = this.transform.parent.gameObject;
        PlayerParent = BulletStorage.gameObject.transform.parent.gameObject;
        RealPlayer = PlayerParent.gameObject.transform.Find("Player").gameObject;

        Destroy(gameObject, 1);
    }

    public void OnCollisionEnter(Collision collider)
    {
        explosionPosition = gameObject.transform.position;

        GameObject explosion = Instantiate(ExplosionVFX, explosionPosition, Quaternion.identity);
        ParticleSystem explosionParticles = explosion.transform.GetChild(0).GetComponent<ParticleSystem>();
        explosionParticles.Play();
        float explositionDuration = explosionParticles.main.duration + explosionParticles.main.startLifetimeMultiplier;
        Destroy(explosion, explositionDuration);

        Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius);

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
