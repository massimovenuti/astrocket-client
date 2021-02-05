using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowShip : MonoBehaviour
{ 
    public GameObject _spaceShip;
    public float SmoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, _spaceShip.transform.position, ref velocity, SmoothTime);
    }
}
