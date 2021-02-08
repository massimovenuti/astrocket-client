using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Luminosity.IO;

public class PlayerControlersRigidBody : MonoBehaviour
{

    public Camera _mainCamera;
    public float rotationSpeed;
    public float _speed = 20.0f;

    private Rigidbody _body;
    private Plane groundPlane;
    private GameObject _spaceShip;

    // Start is called before the first frame update
    void Start( )
    {
        // Find the Spaceship in a player object
        _spaceShip = this.gameObject.transform.Find("Spaceship").gameObject;
        _body = GetComponent<Rigidbody>();
        groundPlane = new Plane(Vector3.up, Vector3.zero);
    }

    private void FixedUpdate( )
    {
        float xAxis = InputManager.GetAxis("Horizontal");
        float zAxis = InputManager.GetAxis("Vertical");

        Vector3 _movement = new Vector3(xAxis, 0f, zAxis);

        LookAt();
        MovePlayer(_movement);
    }


    // Manage the player's movements
    private void MovePlayer(Vector3 direction)
    {
        _body.AddForce(direction * _speed);
    }

    // Manage the position where the spaceship is looking at
    // Works by drawing a raycast on the ground while the spaceship is looking at raycast hit position
    private void LookAt( )
    {
        Ray cameraRay = _mainCamera.ScreenPointToRay(InputManager.mousePosition);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength) + new Vector3(0f, _spaceShip.transform.position.y, 0f);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.red);
            Quaternion lockOnLook = Quaternion.LookRotation(pointToLook + _spaceShip.transform.position);
            _spaceShip.transform.rotation = Quaternion.Slerp(transform.rotation, lockOnLook, rotationSpeed * Time.deltaTime);
        }
    }
}
