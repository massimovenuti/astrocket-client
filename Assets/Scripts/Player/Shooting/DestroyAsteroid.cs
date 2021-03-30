using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEditor;

public class DestroyAsteroid : NetworkBehaviour
{
    [SerializeField] GameObject _asteroidPrefab;

    private GameObject _asteroidToDestroy;

    public GameObject medikit;
    public GameObject akimbo;
    public GameObject shield;
    public GameObject bazooka;
    public GameObject jammer;
    public GameObject drone;

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Asteroid"))
        {
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
    private void DropRemains()
    {
        Vector3 origin = _asteroidToDestroy.transform.position;

        Vector3 newScale = new Vector3(_asteroidToDestroy.transform.localScale.x / 1.5f, _asteroidToDestroy.transform.localScale.y / 1.5f, _asteroidToDestroy.transform.localScale.z / 1.5f);
        int newSize = _asteroidToDestroy.GetComponent<Asteroid>().GetSize() - 1;

        float angle;
        Vector3 angleAxis;
        (Quaternion.identity * Quaternion.Inverse(_asteroidToDestroy.transform.rotation)).ToAngleAxis(out angle, out angleAxis);
        if (Vector3.Angle(Vector3.up, angleAxis) > 90f)
        {
            angle = -angle;
        }
        angle = Mathf.DeltaAngle(0f, angle);
        angle *= -1;

        Vector3 spawnPoint1 = new Vector3(-newScale.x / 16, 0, 0);
        Vector3 spawnPoint2 = new Vector3( newScale.x / 16, 0, 0);

        spawnPoint1 = Quaternion.Euler(0, angle, 0) * spawnPoint1 + origin;
        spawnPoint2 = Quaternion.Euler(0, angle, 0) * spawnPoint2 + origin;

        Quaternion parLa = new Quaternion(_asteroidToDestroy.transform.rotation.x, _asteroidToDestroy.transform.rotation.y, _asteroidToDestroy.transform.rotation.z, _asteroidToDestroy.transform.rotation.w);
        Quaternion nonMaisParLa = new Quaternion(_asteroidToDestroy.transform.rotation.x, _asteroidToDestroy.transform.rotation.y, _asteroidToDestroy.transform.rotation.z, _asteroidToDestroy.transform.rotation.w);

        int r1 = Random.Range(-10, -50);
        int r2 = Random.Range(10, 50);

        parLa *= Quaternion.Euler(Vector3.up * r1);
        nonMaisParLa *= Quaternion.Euler(Vector3.up * r2);

        GameObject remain1 = (GameObject)Instantiate(_asteroidPrefab, spawnPoint1, parLa);
        GameObject remain2 = (GameObject)Instantiate(_asteroidPrefab, spawnPoint2, nonMaisParLa);

        remain1.transform.localScale = newScale;
        remain1.GetComponent<Asteroid>().SetSize(newSize);

        remain2.transform.localScale = newScale;
        remain2.GetComponent<Asteroid>().SetSize(newSize);

        NetworkServer.Destroy(_asteroidToDestroy);

        NetworkServer.Spawn(remain1);
        NetworkServer.Spawn(remain2);
    }

    [Server]
    private void DropPowerUp()
    {
        // 1 chance sur 2 de faire apparaitre
        // un power-up
        if (Random.value >= 0.5)
        {
            // Pourcentage de change d'avoir un power-up
            int drop = Random.Range(1, 100);

            // TODO: change values
            if (drop > 0 && drop <= 35)
                InstantiatePowerUp(medikit);

            else if (drop > 36 && drop <= 55)
                InstantiatePowerUp(shield);

            else if (drop > 56 && drop <= 75)
                Debug.Log("Not implemented yet...");    // BONUS

            else if (drop > 76 && drop <= 85)
                //InstantiatePowerUp(drone);
                Debug.Log("Not Net implemented yet...");

            else if (drop > 86 && drop <= 92)
                InstantiatePowerUp(akimbo);

            else if (drop > 93 && drop <= 99)
                InstantiatePowerUp(bazooka);
            else
                //InstantiatePowerUp(jammer);
                Debug.Log("Le chef est pas sur");
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
