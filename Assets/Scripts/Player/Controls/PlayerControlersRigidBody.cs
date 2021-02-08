using UnityEngine;
using Luminosity.IO;

public class PlayerControlersRigidBody : MonoBehaviour
{

    public float speed = 20.0f;
    public float rotationSpeed = 12f;

    public GameObject _spaceShip;
    public Camera _mainCamera;
    private Rigidbody _body;
    private Plane groundPlane;

    // Start is called before the first frame update
    void Start( )
    {
        _body = GetComponent<Rigidbody>();
        groundPlane = new Plane(Vector3.up, Vector3.zero);
    }

    private void FixedUpdate( )
    {
        float xAxis = InputManager.GetAxis("Horizontal");
        float zAxis = InputManager.GetAxis("Vertical");
        Vector3 mousePos = InputManager.mousePosition;

        Vector3 _movement = new Vector3(xAxis, 0, zAxis);

        LookAt(mousePos);
        MovePlayer(_movement);
    }

    // Manage the player's movements
    private void MovePlayer(Vector3 direction)
    {
        _body.AddForce(direction * speed);
    }

    // Manage the position where the spaceship is looking at
    // Works by drawing a raycast on the ground while the spaceship is looking at raycast hit position
    private void LookAt(Vector3 mousePos)
    {
        Ray cameraRay = _mainCamera.ScreenPointToRay(mousePos);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength) + new Vector3(0f, transform.position.y, 0f);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.red);
            Quaternion lockOnLook = Quaternion.LookRotation(pointToLook - transform.position);
            _spaceShip.transform.rotation = Quaternion.Slerp(transform.rotation, lockOnLook, rotationSpeed * Time.deltaTime);
        }
    }
}