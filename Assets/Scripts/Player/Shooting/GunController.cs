using System.Linq;
using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour
{
    public GameObject bullet;
    public float shootRate = 0.2f;
    public Material bulletMaterial;
    public float shootForce = 3000f;
    public float shootForceRocket = 2000f;
    public string ShootingFrom = "Barrel";
    public string BulletStorageTagName = "BulletStorage";

    public bool akimbo;
    public bool mitraillette;
    public bool bazooka;

    private InputManager _inp;

    private GameObject _barrel;
    private GameObject _barrelAkimbo1;
    private GameObject _barrelAkimbo2;
    public GameObject rocket;

    private GameObject _bulletSpawn;

    private float _lastShootingTimeRef;

    private void Start( )
    {
        _barrel = transform.gameObject.FindObjectByName(ShootingFrom);
        if (_barrel == null)
            Debug.LogError($"GunController : Impossible to find player's barrel");

        _barrelAkimbo1 = transform.gameObject.FindObjectByName("BarrelAkimbo1");
        if (_barrelAkimbo1 == null)
            Debug.LogError($"GunController : Impossible to find player's barrelAkimbo1");

        _barrelAkimbo2 = transform.gameObject.FindObjectByName("BarrelAkimbo2");
        if (_barrelAkimbo2 == null)
            Debug.LogError($"GunController : Impossible to find player's barrelAkimbo2");

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

        akimbo = false;
        mitraillette = false;
        bazooka = false;
    }

    private void Update( )
    {
        if (_inp.IsShooting() && akimbo)
            ShootAkimbo();
        else if (_inp.IsShooting() && bazooka)
            ShootBazooka();
        else if (_inp.IsShooting())
            Shoot();
    }

    private void Shoot( )
    {
        if (Time.time > _lastShootingTimeRef)
        {
            Quaternion rot = _barrel.transform.rotation * Quaternion.Euler(90, 0, 0);
            GameObject go = (GameObject)Instantiate(bullet, _barrel.transform.position, rot);

            go.GetComponent<MeshRenderer>().material = bulletMaterial;
            go.GetComponent<TrailRenderer>().material = bulletMaterial;

            go.transform.parent = _bulletSpawn.transform;
            go.GetComponent<Rigidbody>().AddForce(_barrel.transform.forward * shootForce);

            _lastShootingTimeRef = Time.time + shootRate;
        }
    }

    private void ShootAkimbo( )
    {
        if (Time.time > _lastShootingTimeRef)
        {
            Quaternion rot1 = _barrelAkimbo1.transform.rotation * Quaternion.Euler(90, 0, 0);
            Quaternion rot2 = _barrelAkimbo2.transform.rotation * Quaternion.Euler(90, 0, 0);
            GameObject go1 = (GameObject)Instantiate(bullet, _barrelAkimbo1.transform.position, rot1);
            GameObject go2 = (GameObject)Instantiate(bullet, _barrelAkimbo2.transform.position, rot2);

            go1.GetComponent<MeshRenderer>().material = bulletMaterial;
            go1.GetComponent<TrailRenderer>().material = bulletMaterial;
            go2.GetComponent<MeshRenderer>().material = bulletMaterial;
            go2.GetComponent<TrailRenderer>().material = bulletMaterial;

            go1.transform.parent = _bulletSpawn.transform;
            go1.GetComponent<Rigidbody>().AddForce(_barrelAkimbo1.transform.forward * shootForce);
            go2.transform.parent = _bulletSpawn.transform;
            go2.GetComponent<Rigidbody>().AddForce(_barrelAkimbo2.transform.forward * shootForce);

            _lastShootingTimeRef = Time.time + shootRate;
        }
    }

    private void ShootBazooka( )
    {
        if (Time.time > _lastShootingTimeRef)
        {
            Quaternion rot = _barrel.transform.rotation * Quaternion.Euler(90, 0, 0);
            GameObject go = (GameObject)Instantiate(rocket, _barrel.transform.position, rot);

            go.transform.parent = _bulletSpawn.transform;
            go.GetComponent<Rigidbody>().AddForce(_barrel.transform.forward * shootForceRocket);

            _lastShootingTimeRef = Time.time + shootRate;
        }
    }

    private void PowerUpNewShootRate( )
    {
        mitraillette = true;

        // TODO: change values
        shootRate -= 0.1f;

        StartCoroutine(TimerMitraillette());
    }

    private void PowerUpAkimbo( )
    {
        if (bazooka)
            bazooka = false;
        akimbo = true;

        StartCoroutine(TimerAkimbo());
    }

    private void PowerUpBazooka( )
    {
        if (akimbo)
            akimbo =  false;
        bazooka = true;

        // TODO: change values
        shootRate += 0.2f;

        StartCoroutine(TimerBazooka());
    }

    // Fonction attendant 5 secondes avant de
    // désactiver le power-up akimbo
    private IEnumerator TimerAkimbo( )
    {
        // TODO: change value
        yield return new WaitForSeconds(5);
        akimbo = false;
    }

    // Fonction attendant 5 secondes avant de
    // désactiver le power-up mitraillette
    private IEnumerator TimerMitraillette( )
    {
        // TODO: change value
        yield return new WaitForSeconds(5);
        mitraillette = false;
        shootRate += 0.1f;
    }

    // Fonction attendant 5 secondes avant de
    // désactiver le power-up bazooka
    private IEnumerator TimerBazooka( )
    {
        // TODO: change value
        yield return new WaitForSeconds(15);
        shootRate -= 0.2f;

        bazooka = false;
    }
}