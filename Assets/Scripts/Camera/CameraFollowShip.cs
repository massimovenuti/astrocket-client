using System.Linq;
using UnityEngine;

public class CameraFollowShip : MonoBehaviour
{
    public float SmoothTime = 0.3f;

    private GameObject _spaceShip;
    // TODO : Why a field and not a local variable ? 
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Awake( )
    {
        _spaceShip = GameObject.FindGameObjectsWithTag("Player").First();
    }

    // Update is called once per frame
    void Update( )
    {
        transform.position = Vector3.SmoothDamp(transform.position, _spaceShip.transform.position, ref velocity, SmoothTime);
    }
}
