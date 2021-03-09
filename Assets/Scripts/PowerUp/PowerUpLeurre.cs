using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpLeurre : MonoBehaviour
{

    public GameObject asteroidPrefab;
    public GameObject asteroidStorage;

    private float _behindPlayer = 7f;

    // TODO: change values
    private float _asteroidVelocity = 1f;

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            GameObject asteroidStorage = GameObject.Find("AsteroidStorage");

            // TODO: fix pos
            Vector3 pos = new Vector3(collider.transform.position.x - (collider.transform.forward.x * _behindPlayer), 0, collider.transform.position.z - (collider.transform.forward.z * _behindPlayer))/*.normalized*/;   
            Vector3 dir = - collider.gameObject.GetComponent<Rigidbody>().velocity;

            GameObject go = Instantiate(asteroidPrefab, pos, collider.transform.rotation);
            go.transform.parent = asteroidStorage.transform;

            Rigidbody rb = go.GetComponent<Rigidbody>();
            rb.velocity = dir * _asteroidVelocity;

            Destroy(this.gameObject);
        }
    }
}
