using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AnimationAsteroid : NetworkBehaviour
{
    [SerializeField]
    private float speed = 50f;

    private int clockwise;

    private void Start( )
    {
        clockwise = (Random.value < 0.5f) ? 1 : -1;
        speed += Random.Range(0.0f, 50.0f);
    }

    private void Update( )
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, speed * Time.deltaTime * clockwise));
    }
}
