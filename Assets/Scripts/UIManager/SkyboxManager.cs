using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    private float _rotationSpeed = 1.25f;

    void Update()
    {
        GetComponent<Skybox>().material.SetFloat("_Rotation", Time.time * _rotationSpeed);
    }
}
