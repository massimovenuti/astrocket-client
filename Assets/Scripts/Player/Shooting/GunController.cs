using System.Linq;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject bullet;
    public float shootRate = 0.2f;
    public float shootForce = 3000f;
    public string ShootingFrom = "Barrel";
    public string BulletStorageTagName = "BulletStorage";

    private InputManager _inp;

    private GameObject _barrel;
    private GameObject _bulletSpawn;

    private float _lastShootingTimeRef;

    private void Start( )
    {
        _barrel = transform.gameObject.FindObjectByName(ShootingFrom);
        if (_barrel == null)
            Debug.LogError($"GunController : Impossible to find player's barrel");

        _inp = FindObjectOfType<InputManager>();
        if (_inp == null)
            Debug.LogError($"No InputManager object was found");

        GameObject[] l = GameObject.FindGameObjectsWithTag(BulletStorageTagName);
        if (l.Length == 0)
        {
            Debug.LogError($"There were no GameObjets with tag {BulletStorageTagName} assigned self");
            _bulletSpawn = gameObject;
        }
        else if (l.Length >= 1)
        {
            _bulletSpawn = l.First();
            if (l.Length > 1)
                Debug.LogWarning($"{BulletStorageTagName} had more than one element assigned ({l.Length})");
        }
    }

    private void Update( )
    {
        if (_inp.IsShooting())
            Shoot();
    }

    private void Shoot( )
    {
        if (Time.time > _lastShootingTimeRef)
        {
            Quaternion rot = _barrel.transform.rotation * Quaternion.Euler(90, 0, 0);
            GameObject go = (GameObject)Instantiate(bullet, _barrel.transform.position, rot);

            go.transform.parent = _bulletSpawn.transform;
            go.GetComponent<Rigidbody>().AddForce(_barrel.transform.forward * shootForce);

            _lastShootingTimeRef = Time.time + shootRate;
        }
    }
}
