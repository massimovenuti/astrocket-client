using System.Linq;
using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour
{
    public GameObject bullet;
    public float shootRate = 0.2f;
    public Material bulletMaterial;
    public float shootForce = 3000f;
    public string ShootingFrom = "Barrel";
    public string BulletStorageTagName = "BulletStorage";

    public bool akimbo;
    public bool mitraillette;
    public bool bazooka;
    public bool homing;
    public bool heavy;

    private float _refShootRate;
    private float _mitrailletteShootRate;
    private float _bazookaShootRate;
    private float _heavyLaserShootRate;
    private float _shootForceRocket;
    private float _shootForceHeavyLaser;
    private float _homingShootRate;

    private InputManager _inp;

    private GameObject _barrel;
    private GameObject _barrelAkimbo1;
    private GameObject _barrelAkimbo2;
    public GameObject rocket;

    public GameObject homingMissile;

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
        homing = false;
        heavy = false;
        _refShootRate = shootRate;
        _mitrailletteShootRate = shootRate / 2;
        _bazookaShootRate = shootRate * 2;
        _shootForceRocket = (shootForce * 3) / 2;
        _homingShootRate = shootRate * 3;
        _heavyLaserShootRate = shootRate * 4;
        _shootForceHeavyLaser = shootForce * 2;

    }

    private void Update( )
    {
        if (_inp.IsShooting() && akimbo)
            ShootAkimbo();
        else if (_inp.IsShooting() && bazooka)
            ShootBazooka();
        else if (_inp.IsShooting() && homing)
            ShootHomingMissile();
        else if (_inp.IsShooting() && heavy)
            ShootHeavyLaser();
        else if (_inp.IsShooting())
            Shoot();
    }

    /// <summary>
    /// Fonction de tir de base (instancie un tir)
    /// </summary>
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

    /// <summary>
    /// Instancie, pour chaque barelAkimbo, un tir
    /// </summary>
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

    /// <summary>
    /// Instancie une rocket 
    /// </summary>
    private void ShootBazooka( )
    {
        if (Time.time > _lastShootingTimeRef)
        {
            Quaternion rot = _barrel.transform.rotation * Quaternion.Euler(90, 0, 0);
            GameObject go = (GameObject)Instantiate(rocket, _barrel.transform.position, rot);

            go.transform.parent = _bulletSpawn.transform;
            go.GetComponent<Rigidbody>().AddForce(_barrel.transform.forward * _shootForceRocket);

            _lastShootingTimeRef = Time.time + shootRate;
        }
    }

    /// <summary>
    /// Instancie un missile
    /// </summary>
    private void ShootHomingMissile()
    {
        if (Time.time > _lastShootingTimeRef)
        {
            Quaternion rot = _barrel.transform.rotation * Quaternion.Euler(90, 0, 0);
            GameObject go = (GameObject)Instantiate(homingMissile, _barrel.transform.position, rot);

            go.transform.parent = _bulletSpawn.transform;

            _lastShootingTimeRef = Time.time + shootRate;
        }
    }

    /// <summary>
    /// Instancie un heavy laser (laser puissant)
    /// </summary>
    private void ShootHeavyLaser( )
    {
        if (Time.time > _lastShootingTimeRef)
        {
            Quaternion rot = _barrel.transform.rotation * Quaternion.Euler(90, 0, 0);
            GameObject go = (GameObject)Instantiate(bullet, _barrel.transform.position, rot);

            go.GetComponent<MeshRenderer>().material = bulletMaterial;
            go.GetComponent<TrailRenderer>().material = bulletMaterial;

            go.transform.parent = _bulletSpawn.transform;

            go.tag = "HeavyLaser";

            go.GetComponent<Rigidbody>().AddForce(_barrel.transform.forward * _shootForceHeavyLaser);

            _lastShootingTimeRef = Time.time + shootRate * 2;
        }
    }

    /// <summary>
    /// Augmente la cadence de tir pour le power-up Mitraillette
    /// et désactive tout les autres power-up de tir
    /// </summary>
    public void PowerUpNewShootRate( )
    {
        // on ne peut avoir qu'un power-up de
        // tir à la fois
        if (akimbo)
            akimbo = false;
        else if (bazooka)
            bazooka = false;
        else if (homing)
            homing = false;
        else if (heavy)
            heavy = false;
        
        mitraillette = true;
        shootRate = _mitrailletteShootRate;

        StartCoroutine(TimerMitraillette());
    }

    /// <summary>
    /// Augmente la cadence de tir pour le power-up Akimbo
    /// et désactive tout les autres power-up de tir
    /// </summary>
    public void PowerUpAkimbo( )
    {
        if (bazooka)
            bazooka = false;
        else if (mitraillette)
            mitraillette = false;
        else if (homing)
            homing = false;
        else if (heavy)
            heavy = false;

        akimbo = true;
        shootRate = _refShootRate;

        StartCoroutine(TimerAkimbo());
    }

    /// <summary>
    /// Augmente la cadence de tir pour le power-up Bazooka
    /// et désactive tout les autres power-up de tir
    /// </summary>
    public void PowerUpBazooka( )
    {
        if (akimbo)
            akimbo = false;
        else if (mitraillette)
            mitraillette = false;
        else if (homing)
            homing = false;
        else if (heavy)
            heavy = false;

        bazooka = true;
        shootRate = _bazookaShootRate;

        StartCoroutine(TimerBazooka());
    }

    /// <summary>
    /// Augmente la cadence de tir pour le power-up Homing Missile
    /// et désactive tout les autres power-up de tir
    /// </summary>
    public void PowerUpHomingMissile()
    {
        if (akimbo)
            akimbo = false;
        else if (mitraillette)
            mitraillette = false;
        else if (bazooka)
            bazooka = false;
        else if (heavy)
            heavy = false;

        homing = true;
        shootRate = _homingShootRate;
        StartCoroutine(TimerHomingMissile());
    }

    /// <summary>
    /// Augmente la cadence de tir pour le power-up Heavy laser
    /// et désactive tout les autres power-up de tir
    /// </summary>
    public void PowerUpHeavyLaser( )
    {
        if (akimbo)
            akimbo = false;
        else if (mitraillette)
            mitraillette = false;
        else if (bazooka)
            bazooka = false;
        else if (homing)
            homing = false;

        heavy = true;
        shootRate = _heavyLaserShootRate;
        StartCoroutine(TimerHeavyLaser());
    }

    /// <summary>
    /// Fonction attendant 10 secondes avant de
    /// désactiver le power-up akimbo
    /// </summary>
    private IEnumerator TimerAkimbo( )
    {
        // TODO: change value
        yield return new WaitForSeconds(10);

        akimbo = false;
        shootRate = _refShootRate;
    }

    /// <summary>
    /// Fonction attendant 10 secondes avant de
    /// désactiver le power-up mitraillette
    /// </summary>
    private IEnumerator TimerMitraillette( )
    {
        // TODO: change value
        yield return new WaitForSeconds(10);

        mitraillette = false;
        shootRate = _refShootRate;
    }

    /// <summary>
    /// Fonction attendant 30 secondes avant de
    /// désactiver le power-up bazooka
    /// </summary>
    private IEnumerator TimerBazooka( )
    {
        // TODO: change value
        yield return new WaitForSeconds(30);

        bazooka = false;
        shootRate = _refShootRate;
    }

    /// <summary>
    /// Fonction attendant 30 secondes avant de
    /// désactiver le power-up homing missile
    /// </summary>
    private IEnumerator TimerHomingMissile( )
    {
        // TODO: change value
        yield return new WaitForSeconds(30);
        
        homing = false;
        shootRate = _refShootRate;
        
    }

    /// <summary>
    /// Fonction attendant 15 secondes avant de
    /// désactiver le power-up heavy laser
    /// </summary>
    private IEnumerator TimerHeavyLaser( )
    {
        // TODO: change value
        yield return new WaitForSeconds(15);

        heavy = false;
        shootRate = _refShootRate;

    }

    /// <summary>
    /// Supprime tout les tirs spéciaux liés aux power-up, utilisé quand un joueur meurt
    /// </summary>
    public void ResetPowerUps( )
    {
        if (akimbo)
        {
            akimbo = false;
            shootRate = _refShootRate;
        }
        if (bazooka)
        {
            bazooka = false;
            shootRate = _refShootRate;
        }
        if (mitraillette)
        {
            mitraillette = false;
            shootRate = _refShootRate;
        }
        if (homing)
        {
            homing = false;
            shootRate = _refShootRate;
        }

        if (heavy)
        {
            heavy = false;
            shootRate = _refShootRate;
        }
    }
}