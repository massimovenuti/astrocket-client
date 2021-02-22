using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Movements : MonoBehaviour
{
    private float _forwardSpeed = 5f;
    private float _rotationSpeed = 10f;

    private Camera _mainCamera;
    private Rigidbody _rgbody;
    private InputManager _inp;
    private Plane _groundPlane;

    /// <summary>
    /// Trouver le rigidbody du vaisseau
    /// </summary>
    private void Awake()
    {
        _inp = FindObjectOfType<InputManager>();
        _mainCamera = GameObject.FindGameObjectsWithTag("MainCamera").First().GetComponent<Camera>();
        _rgbody = gameObject.GetComponent<Rigidbody>();
        _groundPlane = new Plane(Vector3.up, Vector3.zero);

    }

    private void Update()
    {
        LookAt();
        if (_inp.IsBoosting())
            _rgbody.AddForce(transform.forward * _forwardSpeed);
    }

    private void LookAt( )
    {
        Vector3 mousePosition = _inp.PointingAt();
        Ray cameraRay = _mainCamera.ScreenPointToRay(mousePosition);

        if (_groundPlane.Raycast(cameraRay, out float rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength) + new Vector3(0f, transform.position.y, 0f);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.red);
            Quaternion lockOnLook = Quaternion.LookRotation(pointToLook - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lockOnLook, _rotationSpeed * Time.deltaTime);
        }
    }
}