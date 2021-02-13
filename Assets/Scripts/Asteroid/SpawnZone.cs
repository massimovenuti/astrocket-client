using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    
    public GameObject _Asteroid;

    public GameObject _SpawningZone;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //retourne le vecteur pour indiquer la zone de spawn
    void EvaluatePosition()
    {
        float xmin = _SpawningZone.GetComponent<SphereCollider>().bounds.min.x;
        float xmax = _SpawningZone.GetComponent<SphereCollider>().bounds.max.x;

        float zmin = _SpawningZone.GetComponent<SphereCollider>().bounds.min.z;
        float zmax = _SpawningZone.GetComponent<SphereCollider>().bounds.max.z;

        float x = Random.Range(xmin, xmax);
        float z = Random.Range(zmin, zmax);

        _SpawningZone.transform.position = new Vector3(x, 0, z);

        //sendmessage _SpawningZone.transform.position
        _Asteroid.SendMessage("DropRemains", new Vector3(x, 0, z));

    }
}
