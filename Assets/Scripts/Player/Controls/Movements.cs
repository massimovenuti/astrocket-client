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
    private ParticleSystem _flameShip1;
    private ParticleSystem _flameShip2;

    private bool _animated;

    /// <summary>
    /// Trouver le rigidbody du vaisseau
    /// </summary>
    private void Awake()
    {
        _inp = FindObjectOfType<InputManager>();
        _mainCamera = GameObject.FindGameObjectsWithTag("MainCamera").First().GetComponent<Camera>();
        _rgbody = gameObject.GetComponent<Rigidbody>();
        _groundPlane = new Plane(Vector3.up, Vector3.zero);
        _flameShip1 = GameObject.Find("flametrail_left").GetComponent<ParticleSystem>();
        _flameShip2 = GameObject.Find("flametrail_right").GetComponent<ParticleSystem>();
        _animated = false;
    }

    private void Update()
    {
        LookAt();
        if (_inp.IsBoosting())
        {
            _rgbody.AddForce(transform.forward * _forwardSpeed);
            if (!_animated)
            {
                _animated = true;
                _flameShip1.Play();
                _flameShip2.Play();
            }
        }

        else if (_animated)
        {
            _animated = false;
            _flameShip1.Stop();
            _flameShip2.Stop();
        }
    }

#if UNITY_ANDROID
    private void LookAt()
    {
        Vector3 mousePosition = _inp.PointingAt();
        if(mousePosition != Vector3.zero)
        {
            mousePosition = _mainCamera.WorldToScreenPoint(transform.position + mousePosition);
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
#else
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
#endif
}