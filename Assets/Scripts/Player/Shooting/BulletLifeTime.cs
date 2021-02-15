using UnityEngine;

public class BulletLifeTime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start( )
    {
        Destroy(gameObject, 1);
    }
}
