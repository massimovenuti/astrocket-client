using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DestroyAsteroid : NetworkBehaviour
{
    [SerializeField] GameObject _asteroidPrefab;

    private GameObject _asteroidToDestroy;

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Asteroid"))
        {
            _asteroidToDestroy = other.gameObject;

            if (_asteroidToDestroy.GetComponent<Asteroid>().GetSize() > 1)
            {
                DropRemains();
            }
            else
            {
                DropPowerUP();
            }

            NetworkServer.Destroy(this.gameObject);
        }
    }

    [Server]
    private void DropRemains()
    {
        Vector3 origin = _asteroidToDestroy.transform.position;

        Vector3 newScale = new Vector3(_asteroidToDestroy.transform.localScale.x / 2, _asteroidToDestroy.transform.localScale.y / 2, _asteroidToDestroy.transform.localScale.z / 2);
        float newMass = _asteroidToDestroy.GetComponent<Rigidbody>().mass / 1.5f;
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
        Debug.Log(r1 + " " + r2);

        parLa *= Quaternion.Euler(Vector3.up * r1);
        nonMaisParLa *= Quaternion.Euler(Vector3.up * r2);

        GameObject remain1 = (GameObject)Instantiate(_asteroidPrefab, spawnPoint1, parLa);
        GameObject remain2 = (GameObject)Instantiate(_asteroidPrefab, spawnPoint2, nonMaisParLa);

        remain1.transform.localScale = newScale;
        remain1.GetComponent<Rigidbody>().mass = newMass;
        remain1.GetComponent<Asteroid>().SetSize(newSize);

        remain2.transform.localScale = newScale;
        remain2.GetComponent<Rigidbody>().mass = newMass;
        remain2.GetComponent<Asteroid>().SetSize(newSize);

        NetworkServer.Destroy(_asteroidToDestroy);

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

        NetworkServer.Destroy(_asteroidToDestroy);
    }

    /*
     private void DropRemains(Vector3 vec, Vector3 origin)
    {
        float remainSpeed = 1.5f;

        float angle1 = Random.Range(Mathf.PI / 16, Mathf.PI / 8);
        float angle2 = Random.Range(Mathf.PI / 16, Mathf.PI / 8);

        GameObject remain1 = (GameObject)Instantiate(_asteroidPrefab, spawningRemains.position, Random.rotation);
        GameObject remain2 = (GameObject)Instantiate(_asteroidPrefab, spawningRemains.position, Random.rotation);

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
