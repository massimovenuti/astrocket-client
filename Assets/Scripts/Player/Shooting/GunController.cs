using System.Linq;
using UnityEngine;
using Mirror;

public class GunController : NetworkBehaviour
{
    public GameObject bullet;
    public float shootRate = 0.2f;
    public Material bulletMaterial;
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
        if (!isLocalPlayer)
        {
            return;
        }

        if (Time.time > _lastShootingTimeRef && _inp.IsShooting())
        {
            CmdShoot();
            _lastShootingTimeRef = Time.time + shootRate;
        }
    }

    [Command]
    private void CmdShoot( )
    {
        if (Time.time > _lastShootingTimeRef)
        {
            GameObject go = (GameObject)Instantiate(bullet, _barrel.transform.position, _barrel.transform.rotation);

            go.GetComponent<MeshRenderer>().material = bulletMaterial;
            go.GetComponent<TrailRenderer>().material = bulletMaterial;
            go.GetComponent<Bullet>().ownerId = netId;

            //go.transform.parent = _bulletSpawn.transform;
            NetworkServer.Spawn(go);

            _lastShootingTimeRef = Time.time + shootRate;
        }
    }
}
