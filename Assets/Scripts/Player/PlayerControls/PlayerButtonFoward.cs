using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerButtonFoward : MonoBehaviour
{
    [SerializeField]
    private float fowardSpeed = 5.0f;

    private Rigidbody rgbody;

    /// <summary>
    /// Trouver le rigidbody du vaisseau
    /// </summary>
    void Start()
    {
        rgbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rgbody.AddForce(transform.forward * fowardSpeed);
            //transform.position += transform.forward * Time.deltaTime * fowardSpeed; //move foward but no acceleration
        }
    }
}