using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBullet : MonoBehaviour
{
    public Collider target;

    private Rigidbody _rb;

    private Vector3 _rotationAmount;
    private Vector3 _direction;

    private float _rotateMissileSpeed = 4f;
    private float _force = 35f;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        Destroy(gameObject, 3);
    }

    // Update is called once per frame
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
