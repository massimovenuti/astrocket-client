using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject _spaceShip;
    public Camera _mainCamera;
    
    private Plane groundPlane;

    private float _speed = 10.0f;
    public float rotationSpeed;
    
    // Start is called before the first frame update
    private void Start()
    {
        groundPlane = new Plane(Vector3.up, Vector3.zero);
    }

    // Update is called once per frame
    private void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        float sideInput = Input.GetAxis("Horizontal");

        MovePlayer(forwardInput, sideInput);
        LookAt();
    }

    // Manage the player's movements
    private void MovePlayer(float forwardInput, float sideInput)
    {
        transform.position += forwardInput * _speed * Time.deltaTime * transform.forward;
        transform.position += sideInput * _speed * Time.deltaTime * transform.right;
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
