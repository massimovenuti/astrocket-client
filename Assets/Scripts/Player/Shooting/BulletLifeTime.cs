using UnityEngine;

public class BulletLifeTime : MonoBehaviour
{

    // Start is called before the first frame update
    public void Start( )
    {
        Destroy(gameObject, 1);
    }
}
