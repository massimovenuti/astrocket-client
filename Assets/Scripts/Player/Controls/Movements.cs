using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class Movements : MonoBehaviour
{
    private float _forwardSpeed = 5f;
    private float _rotationSpeed = 10f;

    private Camera _mainCamera;
    private Rigidbody _rgbody;
    private InputManager _inp;
    private Plane _groundPlane;

    private Queue<float> _interpolation;
    private int _frameNum = 4;

    private Animator PlayerController;
    GameObject _player;

    /// <summary>
    /// Trouver le rigidbody du vaisseau
    /// </summary>
    private void Awake()
    {
        _inp = FindObjectOfType<InputManager>();
        _mainCamera = GameObject.FindGameObjectsWithTag("MainCamera").First().GetComponent<Camera>();
        _rgbody = gameObject.GetComponent<Rigidbody>();
        _groundPlane = new Plane(Vector3.up, Vector3.zero);
        _player = GameObject.FindGameObjectsWithTag("Player").First();
        _interpolation = new Queue<float>(_frameNum);
    }

    private void Update()
    {
        Vector3 _lastRotate = _rgbody.transform.eulerAngles;

        LookAt();

        Vector3 _currRotate = _rgbody.transform.eulerAngles;

        if (_inp.IsBoosting())
            _rgbody.AddForce(transform.forward * _forwardSpeed);        

        PlayerController = _player.FindObjectByName("SpaceFighter").GetComponent<Animator>();
        
        _lastRotate.z = 1;
        _currRotate.z = 1;
        
        _lastRotate.x = 1;
        _currRotate.x = 1;

        Vector3 crossP = Vector3.Cross(_lastRotate, _currRotate).normalized;

        if (_interpolation.Count != _frameNum)
        {
            _interpolation.Enqueue((float)System.Math.Round(crossP.z * 100f) / 100f);
        }
        if (_interpolation.Count == _frameNum)
        {
            _interpolation.Dequeue();
            _interpolation.Enqueue((float)System.Math.Round(crossP.z * 100f) / 100f);

            bool checkVal = true;

            for (int i = 1; i < _frameNum && checkVal; i++)
            {
                if (_interpolation.ElementAt(0) != _interpolation.ElementAt(i))
                    checkVal = false;
            }

            if (checkVal)
            {
                if (_interpolation.ElementAt(0) < 0) //turns left
                {
                    PlayerController.SetBool("BoolLeft", true);
                    PlayerController.SetBool("BoolRight", false);
                }
                else if (_interpolation.ElementAt(0) > 0) //turn right
                {
                    PlayerController.SetBool("BoolRight", true);
                    PlayerController.SetBool("BoolLeft", false);
                }

                else //idle
                {
                    PlayerController.SetBool("BoolRight", false);
                    PlayerController.SetBool("BoolLeft", false);
                }
            }
        }

        /*
        if (crossP.z < 0) //turns left _lastRotate.y - _currRotate.y >= 0.2
        {
            PlayerController.SetBool("BoolLeft", true);
            PlayerController.SetBool("BoolRight", false);
        }
        else if (crossP.z > 0) //turn right _currRotate.y - _lastRotate.y >= 0.2
        {
            PlayerController.SetBool("BoolRight", true);
            PlayerController.SetBool("BoolLeft", false);
        }

        else //idle
        {
            PlayerController.SetBool("BoolRight", false);
            PlayerController.SetBool("BoolLeft", false);
        }
        */

        Debug.Log(_interpolation.ElementAt(0) + " " + _interpolation.ElementAt(1) + " " + _interpolation.ElementAt(2) + " " + _interpolation.ElementAt(3));

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