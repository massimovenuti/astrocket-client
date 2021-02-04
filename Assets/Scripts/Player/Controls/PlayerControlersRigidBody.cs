using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlersRigidBody : MonoBehaviour
{
    public GameObject _spaceShip;
    public Camera _mainCamera;
    public float _speed = 20.0f;
    public Rigidbody _body;
    public Vector3 _movement;
    private Plane groundPlane;
    public float rotationSpeed;


    
    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody>();
        groundPlane = new Plane(Vector3.up, Vector3.zero);
    }

    private void FixedUpdate(){
    
        _movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
       
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
