using System.Linq;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    private GameObject spaceShip;
    [SerializeField]
    private float _smoothTime = 0.1f;
    private Vector3 _velocity = Vector3.zero;

    private void Start( )
    {
        // TODO : Add error checking here
        spaceShip = GameObject.FindGameObjectsWithTag("Player").First(); 
    }

    private void Update()
    {
        // TODO : if nothing is added make the function a lambda (=>)
        transform.position = Vector3.SmoothDamp(transform.position, spaceShip.transform.position, ref _velocity, _smoothTime);
    }
}
