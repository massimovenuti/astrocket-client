using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEditor;

public class DestroyAsteroid : NetworkBehaviour
{
    [SerializeField] GameObject _asteroidPrefab;

    public GameObject DustVFX;

    [SerializeField] GameObject medikit;
    [SerializeField] GameObject akimbo;
    [SerializeField] GameObject shield;
    [SerializeField] GameObject bazooka;
    [SerializeField] GameObject heavy;
    [SerializeField] GameObject homing;

    private GameObject _asteroidToDestroy;

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Asteroid"))
        {

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject p in players)
            {
                if (p.GetComponent<NetworkIdentity>().netId == GetComponent<Ammo>().ownerId)
                {
                    p.GetComponent<PlayerScore>().addAsteroid();
                    break;
                }
            }

            AsteroidDestruction(other.gameObject);
            if (!this.CompareTag("Rocket"))
            {
                NetworkServer.Destroy(this.gameObject);
            }
        }
    }

    [Server]
    public void AsteroidDestruction(GameObject asteroidToDestroy)
    {
        _asteroidToDestroy = asteroidToDestroy;
        _asteroidToDestroy.GetComponent<Collider>().enabled = false;

        RpcParticuleDust(_asteroidToDestroy.transform.position);

        if (_asteroidToDestroy.GetComponent<Asteroid>().GetSize() > 1)
        {
            DropRemains();
        }
        else
        {
            DropPowerUp();
        }
    }

    [Server]
    private void DropRemains( )
    {
        Vector3 origin = _asteroidToDestroy.transform.position;

        Vector3 newScale = new Vector3(_asteroidToDestroy.transform.localScale.x / 1.5f, _asteroidToDestroy.transform.localScale.y / 1.5f, _asteroidToDestroy.transform.localScale.z / 1.5f);
        int newSize = _asteroidToDestroy.GetComponent<Asteroid>().GetSize() - 1;

        float angle;
        Vector3 angleAxis;
        (Quaternion.identity * Quaternion.Inverse(_asteroidToDestroy.transform.rotation)).ToAngleAxis(out angle, out angleAxis);
        if (Vector3.Angle(Vector3.up, angleAxis) < 90f)
        {
            angle = -angle;
        }
        angle = Mathf.DeltaAngle(0f, angle);

        Vector3 spawnPoint1 = Quaternion.Euler(0, angle, 0) * new Vector3(-newScale.x * 2, 0, 0) + origin;
        Vector3 spawnPoint2 = Quaternion.Euler(0, angle, 0) * new Vector3( newScale.x * 2, 0, 0) + origin;

        Quaternion parLa = new Quaternion(_asteroidToDestroy.transform.rotation.x, _asteroidToDestroy.transform.rotation.y, _asteroidToDestroy.transform.rotation.z, _asteroidToDestroy.transform.rotation.w);
        Quaternion nonMaisParLa = new Quaternion(_asteroidToDestroy.transform.rotation.x, _asteroidToDestroy.transform.rotation.y, _asteroidToDestroy.transform.rotation.z, _asteroidToDestroy.transform.rotation.w);

        NetworkServer.Destroy(_asteroidToDestroy);

        parLa *= Quaternion.Euler(Vector3.up * Random.Range(-10, -50));
        nonMaisParLa *= Quaternion.Euler(Vector3.up * Random.Range(10, 50));

        GameObject remain1 = (GameObject)Instantiate(_asteroidPrefab, spawnPoint1, parLa);
        GameObject remain2 = (GameObject)Instantiate(_asteroidPrefab, spawnPoint2, nonMaisParLa);

        remain1.transform.localScale = newScale;
        remain1.GetComponent<Asteroid>().SetSize(newSize);

        remain2.transform.localScale = newScale;
        remain2.GetComponent<Asteroid>().SetSize(newSize);

        NetworkServer.Spawn(remain1);
        NetworkServer.Spawn(remain2);
    }

    /// <summary>
    /// Instanciation de l'animation des particules de poussières quand un astéroide est dértuit
    /// </summary>
    [ClientRpc]
    private void RpcParticuleDust(Vector3 dustPosition)
    {
        GameObject dust = Instantiate(DustVFX, dustPosition, Quaternion.identity);
        ParticleSystem dustParticles = dust.transform.GetChild(0).GetComponent<ParticleSystem>();
        dustParticles.Play();
        float dustDuration = dustParticles.main.duration + dustParticles.main.startLifetimeMultiplier;
        Destroy(dust, dustDuration);
    }

    [Server]
    private void DropPowerUp( )
    {
        // 1 chance sur 2 de faire apparaitre
        // un power-up
        if (Random.value >= 0.5)
        {
            // Pourcentage de change d'avoir un power-up
            int drop = Random.Range(1, 100);

            // TODO: change values
            if (drop <= 20)
                InstantiatePowerUp(medikit);
            else if (drop <= 40)
                InstantiatePowerUp(shield);
            else if (drop <= 60)
                InstantiatePowerUp(akimbo);
            else
                InstantiatePowerUp(bazooka);
        }

        NetworkServer.Destroy(_asteroidToDestroy);
    }

    /// <summary>
    /// Fonction instanciant un power-up
    /// </summary>
    [Server]
    private void InstantiatePowerUp(GameObject powerUp)
    {
        GameObject PowerUp = (GameObject)Instantiate(powerUp, _asteroidToDestroy.GetComponent<Transform>().position, Quaternion.Euler(0, 0, 0));
        NetworkServer.Spawn(PowerUp);
    }
}
