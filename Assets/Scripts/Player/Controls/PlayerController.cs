using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject _spaceShip;
    public Camera _mainCamera;

    private float _speed = 10.0f;
    
    // Start is called before the first frame update
    private void Start()
    {

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
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if(groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.red);
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }
}
