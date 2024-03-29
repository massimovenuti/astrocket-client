﻿using System.Collections;
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

    private ParticleSystem _flameShip1;
    private ParticleSystem _flameShip2;

    private Queue<float> _interpolation;
    private int _frameNum = 4;

    private Animator PlayerController;

    private bool _animated;


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
    private void Awake( )
    {
        _inp = InputManager.InputManagerInst;
        //_mainCamera = GameObject.FindGameObjectsWithTag("MainCamera").First().GetComponent<Camera>();
        _rgbody = gameObject.GetComponent<Rigidbody>();
        _groundPlane = new Plane(Vector3.up, Vector3.zero);

        // GET IN CHILDREN
        _mainCamera = gameObject.GetComponentInChildren<Camera>();


        _flameShip1 = transform.Find("SpaceFighter/boosters1/flametrail_left").GetComponent<ParticleSystem>();
        _flameShip2 = transform.Find("SpaceFighter/boosters1/flametrail_right").GetComponent<ParticleSystem>();
        _flameShip1.Stop();
        _flameShip2.Stop();
        _animated = false;

        _interpolation = new Queue<float>(_frameNum);
    }

    private void FixedUpdate( )
    {
        Vector3 _lastRotate = _rgbody.transform.eulerAngles;

        if (isLocalPlayer)
        {
            LookAt();
            Vector3 _currRotate = _rgbody.transform.eulerAngles;
            Spin(_lastRotate, _currRotate);
            CmdSpin(_lastRotate, _currRotate);
            if (_inp.IsBoosting())
            {
                _rgbody.AddForce(transform.forward * _forwardSpeed * _multiplierSpeed);
                if (!_animated)
                {
                    _animated = true;
                    CmdBoost(true);
                }
            }
            else if (_animated)
            {
                _animated = false;
                CmdBoost(false);
            }
        }
    }

    private void Spin(Vector3 _lastRotate, Vector3 _currRotate)
    {
        PlayerController = this.gameObject.transform.Find("SpaceFighter").GetComponent<Animator>();

        _lastRotate.z = 1;
        _currRotate.z = 1;

        _lastRotate.x = 1;
        _currRotate.x = 1;

        _lastRotate.y = (int)_lastRotate.y;
        _currRotate.y = (int)_currRotate.y;

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
    }

    [Command]
    private void CmdBoost(bool boost)
    {
        RpcBoost(boost);
    }

    [ClientRpc]
    private void RpcBoost(bool boost)
    {
            if (boost)
            {
                _flameShip1.Play();
                _flameShip2.Play();
            }
            else
            {
                _flameShip1.Stop();
                _flameShip2.Stop();
            }
    }

    [Command]
    private void CmdSpin(Vector3 _lastRotate, Vector3 _currRotate)
    {
        RpcSpin(_lastRotate, _currRotate);
    }

    [ClientRpc]
    private void RpcSpin(Vector3 _lastRotate, Vector3 _currRotate)
    {
        if (!isLocalPlayer)
        {
            Spin(_lastRotate, _currRotate);
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

    [TargetRpc]
    public void RpcResetVelocity( )
    {
        if (isLocalPlayer)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

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