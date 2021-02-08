using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlersRigidBody : MonoBehaviour
{
    public GameObject _spaceShip;
    public Camera _mainCamera;

    public float _speed = 20.0f;
    public float rotationSpeed;

    private Rigidbody _body;
    private Plane groundPlane;
    
    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody>();
        groundPlane = new Plane(Vector3.up, Vector3.zero);
    }

    private void FixedUpdate()
    {  
        float xAxis = Input.GetAxis("Horizontal");
        float zAxis = Input.GetAxis("Vertical");
        
        Vector3 _movement = new Vector3(xAxis, 0, zAxis);
       
        LookAt();
        MovePlayer(_movement);
    }


    // Manage the player's movements
    private void MovePlayer(Vector3 direction)
    {
       _body.AddForce(direction*_speed);
    }

    // Manage the position where the spaceship is looking at
    // Works by drawing a raycast on the ground while the spaceship is looking at raycast hit position
    private void LookAt()
    {
        Ray cameraRay = _mainCamera.ScreenPointToRay(Input.mousePosition); 
        float rayLength;

        if(groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength) + new Vector3(0f, transform.position.y, 0f);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.red);
            //transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            Quaternion lockOnLook = Quaternion.LookRotation(pointToLook - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lockOnLook, rotationSpeed * Time.deltaTime);
        }
    }
}
