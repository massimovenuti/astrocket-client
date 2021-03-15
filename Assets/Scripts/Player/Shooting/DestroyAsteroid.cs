using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DestroyAsteroid : NetworkBehaviour
{
    private GameObject asteroidToDestroy;
    public GameObject asteroidPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            asteroidToDestroy = collision.gameObject;
            AsteroidDestruction();
        }
    }

    [Server]
    private void AsteroidDestruction()
    {
        NetworkServer.Destroy(this.gameObject);

        if (asteroidToDestroy.GetComponent<Asteroid>().GetSize() >= 1)
        {
            asteroidToDestroy.GetComponent<Asteroid>().DecreaseSize();
            DropRemains();
        }
        else
        {    
            DropPowerUP();
        }
    }

    [Server]
    private void DropRemains()
    {
        Vector3 vec = asteroidToDestroy.GetComponent<Rigidbody>().velocity;
        Vector3 origin = asteroidToDestroy.transform.localPosition;

        float spawnPoint1_X = Mathf.Cos(Mathf.PI / 2) * vec.x - Mathf.Sin(Mathf.PI / 2) * vec.z;
        float spawnPoint1_Z = Mathf.Sin(Mathf.PI / 2) * vec.x + Mathf.Cos(Mathf.PI / 2) * vec.z;
        float spawnPoint2_X = Mathf.Cos(-Mathf.PI / 2) * vec.x - Mathf.Sin(-Mathf.PI / 2) * vec.z;
        float spawnPoint2_Z = Mathf.Sin(-Mathf.PI / 2) * vec.x + Mathf.Cos(-Mathf.PI / 2) * vec.z;

        Vector3 spawnPoint1 = new Vector3(spawnPoint1_X, 0, spawnPoint1_Z).normalized * (asteroidToDestroy.transform.localScale.x / 20) + origin;
        Vector3 spawnPoint2 = new Vector3(spawnPoint2_X, 0, spawnPoint2_Z).normalized * (asteroidToDestroy.transform.localScale.x / 20) + origin;

        Quaternion parLa = new Quaternion(asteroidToDestroy.transform.rotation.x, asteroidToDestroy.transform.rotation.y + Random.Range(Mathf.PI / 16, Mathf.PI / 8), asteroidToDestroy.transform.rotation.z, asteroidToDestroy.transform.rotation.w);
        Quaternion nonMaisParLa = new Quaternion(asteroidToDestroy.transform.rotation.x, asteroidToDestroy.transform.rotation.y + Random.Range(Mathf.PI / 16, Mathf.PI / 8), asteroidToDestroy.transform.rotation.z, asteroidToDestroy.transform.rotation.w);

        GameObject remain1 = (GameObject)Instantiate(asteroidPrefab, spawnPoint1, parLa);
        GameObject remain2 = (GameObject)Instantiate(asteroidPrefab, spawnPoint2, nonMaisParLa);

        NetworkServer.Destroy(asteroidToDestroy);

        NetworkServer.Spawn(remain1);
        NetworkServer.Spawn(remain2);
    }

    [Server]
    private void DropPowerUP( )
    {
        int dropRate = Random.Range(1, 100);

        if (dropRate <= 20)
        {
            if (dropRate <= 10)
                Debug.Log("Power-up : Regen vie");

            if (dropRate > 10)
                Debug.Log("Power-up : Shield");
        }
        else
        {
            Debug.Log("Pas de Power-up");
        }

        NetworkServer.Destroy(asteroidToDestroy);
    }

    /*
     private void DropRemains(Vector3 vec, Vector3 origin)
    {
        float remainSpeed = 1.5f;

        float angle1 = Random.Range(Mathf.PI / 16, Mathf.PI / 8);
        float angle2 = Random.Range(Mathf.PI / 16, Mathf.PI / 8);

        GameObject remain1 = (GameObject)Instantiate(asteroidPrefab, spawningRemains.position, Random.rotation);
        GameObject remain2 = (GameObject)Instantiate(asteroidPrefab, spawningRemains.position, Random.rotation);

        Rigidbody rb1 = remain1.GetComponent<Rigidbody>();
        Rigidbody rb2 = remain2.GetComponent<Rigidbody>();

        remain1.GetComponent<DestroyAsteroid>().inMapBounds = inMapBounds;
        remain2.GetComponent<DestroyAsteroid>().inMapBounds = inMapBounds;

        remain1.transform.parent = _asteroidStorage.transform;
        remain2.transform.parent = _asteroidStorage.transform;

        float spawnPoint1_X = Mathf.Cos(Mathf.PI / 2) * vec.x - Mathf.Sin(Mathf.PI / 2) * vec.z;
        float spawnPoint1_Z = Mathf.Sin(Mathf.PI / 2) * vec.x + Mathf.Cos(Mathf.PI / 2) * vec.z;
        float spawnPoint2_X = Mathf.Cos(-Mathf.PI / 2) * vec.x - Mathf.Sin(-Mathf.PI / 2) * vec.z;
        float spawnPoint2_Z = Mathf.Sin(-Mathf.PI / 2) * vec.x + Mathf.Cos(-Mathf.PI / 2) * vec.z;

        Vector3 spawnPoint1 = new Vector3 (spawnPoint1_X, 0, spawnPoint1_Z).normalized * (remain1.transform.localScale.x / 20) + origin;
        Vector3 spawnPoint2 = new Vector3(spawnPoint2_X, 0, spawnPoint2_Z).normalized * (remain1.transform.localScale.x / 20) + origin;

        remain1.transform.position = spawnPoint1;
        remain2.transform.position = spawnPoint2;

        remain1.transform.localScale = new Vector3(remain1.transform.localScale.x / 2, remain1.transform.localScale.y / 2, remain1.transform.localScale.z / 2);
        remain2.transform.localScale = new Vector3(remain2.transform.localScale.x / 2, remain2.transform.localScale.y / 2, remain2.transform.localScale.z / 2);

        rb1.mass /= 2;
        rb2.mass /= 2;

        float velocity1_X = Mathf.Cos(angle1) * vec.x - Mathf.Sin(angle1) * vec.z;
        float velocity1_Z = Mathf.Sin(angle1) * vec.x + Mathf.Cos(angle1) * vec.z;
        float velocity2_X = Mathf.Cos(-angle2) * vec.x - Mathf.Sin(-angle2) * vec.z;
        float velocity2_Z = Mathf.Sin(-angle2) * vec.x + Mathf.Cos(-angle2) * vec.z;

        rb1.velocity = (new Vector3(velocity1_X, 0, velocity1_Z) * remainSpeed);
        rb2.velocity = (new Vector3(velocity2_X, 0, velocity2_Z) * remainSpeed);

        float rotForce_tmp = (remain1.GetComponent<DestroyAsteroid>().size == 2) ? 5000 : 1000;

        rb1.AddTorque(transform.up * rotForce_tmp * ((Random.value < 0.5f) ? 1 : -1));
        rb2.AddTorque(transform.up * rotForce_tmp * ((Random.value < 0.5f) ? 1 : -1));

        NetworkServer.Spawn(remain1, remain2);
    } 
    */
}
