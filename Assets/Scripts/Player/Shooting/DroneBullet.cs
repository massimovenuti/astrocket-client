using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DroneBullet : NetworkBehaviour
{
    public Collider target;

    private Rigidbody _rb;

    private Vector3 _rotationAmount;
    private Vector3 _direction;

    private float _rotateMissileSpeed = 4f;
    private float _force = 35f;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    [ServerCallback]
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Fonction modifiant la trajectoire du laser
    /// tant que sa cible n'est pas détruite
    /// </summary>
    [ServerCallback]
    void FixedUpdate()
    {
        if (target != null)
        {
            _direction = target.transform.position - _rb.position;
            _direction.Normalize();
            _rotationAmount = Vector3.Cross(transform.up, _direction);
            _rb.angularVelocity = _rotationAmount * _rotateMissileSpeed;
            _rb.velocity = transform.up * _force;
        }
    }
}
