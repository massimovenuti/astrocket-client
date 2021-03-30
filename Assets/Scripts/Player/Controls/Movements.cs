using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;

public class Movements : NetworkBehaviour
{
    private float _forwardSpeed = 5f;
    private float _rotationSpeed = 10f;

    private float _multiplierSpeed = 2.4f; //< 60 FixedUpdate/s * 2.4 => 144Hz speed

    private Camera _mainCamera;
    private Rigidbody _rgbody;
    private InputManager _inp;
    private Plane _groundPlane;

    public bool flash;
    public bool slow;
    private float _refSpeed;
    private float _powerUpSpeed;
    private float _powerUpSlowSpeed;


    // Start is called before the first frame update
    private void Start( )
    {
        flash = false;
        slow = false;
        _refSpeed = _forwardSpeed;
        _powerUpSpeed = _forwardSpeed * 2;
        _powerUpSlowSpeed = _forwardSpeed / 2;
    }

    /// <summary>
    /// Trouver le rigidbody du vaisseau
    /// </summary>
    private void Awake()
    {
        _inp = FindObjectOfType<InputManager>();
        //_mainCamera = GameObject.FindGameObjectsWithTag("MainCamera").First().GetComponent<Camera>();
        _rgbody = gameObject.GetComponent<Rigidbody>();
        _groundPlane = new Plane(Vector3.up, Vector3.zero);

        // GET IN CHILDREN
        _mainCamera = gameObject.GetComponentInChildren<Camera>();
    }
    
    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            LookAt();
            if (_inp.IsBoosting())
            {
                _rgbody.AddForce(transform.forward * _forwardSpeed * _multiplierSpeed);
            }
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

    /// <summary>
    /// Augmente la vitesse du vaiseau
    /// </summary>
    public void PowerUpFlash( )
    {
        //si on est ralentit, on repasse à la vitesse normale
        if (slow)
        {
            slow = false;
            _forwardSpeed = _refSpeed;
            return;
        }

        flash = true;
        _forwardSpeed = _powerUpSpeed;

        StartCoroutine(TimerFlash());
    }

    /// <summary>
    /// Diminue la vitesse du vaiseau
    /// </summary>
    public void PowerUpSlowness( )
    {
        //si on a le power-up "flash", on repasse à la vitesse normale
        if (flash)
        {
            flash = false;
            _forwardSpeed = _refSpeed;
            return;
        }

        slow = true;
        _forwardSpeed = _powerUpSlowSpeed;

        StartCoroutine(TimerSlowness());
    }


    // Fonction attendant 10 secondes avant de
    // désactiver le power-up de vitesse
    private IEnumerator TimerFlash( )
    {
        // TODO: change value
        yield return new WaitForSeconds(10);

        flash = false;
        _forwardSpeed = _refSpeed;
    }

    // Fonction attendant 10 secondes avant de
    // désactiver le power-up de ralentissement
    private IEnumerator TimerSlowness( )
    {
        // TODO: change value
        yield return new WaitForSeconds(10);

        slow = false;
        _forwardSpeed = _refSpeed;
    }

    /// <summary>
    /// Supprime tout les états liés aux power-up 
    /// et réinitialise la vitesse:
    /// utilisé quand un joueur meurt
    /// </summary>
    public void ResetPowerUps( )
    {
        flash = false;
        slow = false;
        _forwardSpeed = _refSpeed;
    }
}