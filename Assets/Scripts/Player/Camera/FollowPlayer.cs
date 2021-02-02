using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject _spaceShip;
    public float _smoothTime = 0.1f;

    private Vector3 _velocity = Vector3.zero;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, _spaceShip.transform.position, ref _velocity, _smoothTime);
    }
}
