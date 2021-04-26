using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 1.25f;

    private void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, _rotationSpeed * Time.deltaTime));
    }
}
