using UnityEngine;

public class BulletLifeTime : MonoBehaviour
{
    /// <summary>
    /// Start is called before the first frame update
    /// </summary
    public void Start( )
    {
        // la laser a une durée de vie de 1 seconde
        Destroy(gameObject, 1);
    }
}
