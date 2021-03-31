using System.Linq;
using UnityEngine;
using System.Collections;
using Mirror;

public class GunController : NetworkBehaviour
{
    public GameObject bullet;

    [SyncVar]
    public float shootRate = 0.2f;
    
    public Material bulletMaterial;
    public float shootForce = 3000f;
    public string ShootingFrom = "Barrel";
    public string BulletStorageTagName = "BulletStorage";

    private bool _akimbo, _mitraillette, _bazooka, _homing, _heavy;

    private float _refShootRate, _mitrailletteShootRate, _bazookaShootRate, 
                  _heavyLaserShootRate, _homingShootRate;

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

        if (isServer)
        {
            _akimbo = _mitraillette = _bazooka = _homing = _heavy = false;
        }

        _refShootRate = shootRate;
        _mitrailletteShootRate = shootRate / 2;
        _bazookaShootRate = shootRate * 2;
        _homingShootRate = shootRate * 3;
        _heavyLaserShootRate = shootRate * 4;
    }

    private void Update( )
    {
        if (isLocalPlayer)
        {
            if (Time.time > _lastShootingTimeRef && _inp.IsShooting())
            {
                CmdShoot();
                _lastShootingTimeRef = Time.time + shootRate;
            }
        }
    }

    [Command]
    private void CmdShoot()
    {
        if (_akimbo)
            ShootAkimbo();
        else if (_bazooka)
            ShootBazooka();
        else if (_homing)
            ShootHomingMissile();
        else if (_heavy)
            ShootHeavyLaser();
        else
            Shoot();
    }

    /// <summary>
    /// Fonction de tir de base (instancie un tir)
    /// </summary>
    [Server]
    private void Shoot( )
    {
        GameObject go = (GameObject)Instantiate(bullet, _barrel.transform.position, _barrel.transform.rotation);

        go.GetComponent<MeshRenderer>().material = bulletMaterial;
        go.GetComponent<TrailRenderer>().material = bulletMaterial;
        go.GetComponent<Ammo>().ownerId = netId;

        go.transform.parent = _bulletSpawn.transform;
        
        NetworkServer.Spawn(go);
    }

    /// <summary>
    /// Instancie, pour chaque barel_akimbo, un tir
    /// </summary>
    [Server]
    private void ShootAkimbo( )
    {
        GameObject go1 = (GameObject)Instantiate(bullet, _barrelAkimbo1.transform.position, _barrelAkimbo1.transform.rotation);
        GameObject go2 = (GameObject)Instantiate(bullet, _barrelAkimbo2.transform.position, _barrelAkimbo2.transform.rotation);

        go1.GetComponent<MeshRenderer>().material = bulletMaterial;
        go1.GetComponent<TrailRenderer>().material = bulletMaterial;
        go1.GetComponent<Ammo>().ownerId = netId;
        go2.GetComponent<MeshRenderer>().material = bulletMaterial;
        go2.GetComponent<TrailRenderer>().material = bulletMaterial;
        go2.GetComponent<Ammo>().ownerId = netId;

        go1.transform.parent = _bulletSpawn.transform;
        go2.transform.parent = _bulletSpawn.transform;

        NetworkServer.Spawn(go1);
        NetworkServer.Spawn(go2);
    }

    /// <summary>
    /// Instancie une rocket 
    /// </summary>
    [Server]
    private void ShootBazooka( )
    {
        GameObject go = (GameObject)Instantiate(rocket, _barrel.transform.position, _barrel.transform.rotation);
        
        go.GetComponent<Ammo>().ownerId = netId;
        go.transform.parent = _bulletSpawn.transform;

        NetworkServer.Spawn(go);
    }

    /// <summary>
    /// Instancie un missile
    /// </summary>
    [Server]
    private void ShootHomingMissile()
    {
        GameObject go = (GameObject)Instantiate(homingMissile, _barrel.transform.position, _barrel.transform.rotation);

        go.GetComponent<Ammo>().ownerId = netId;
        go.transform.parent = _bulletSpawn.transform;

        NetworkServer.Spawn(go);
    }

    /// <summary>
    /// Instancie un _heavy laser (laser puissant)
    /// </summary>
    [Server]
    private void ShootHeavyLaser( )
    {
        GameObject go = (GameObject)Instantiate(bullet, _barrel.transform.position, _barrel.transform.rotation);

        Vector3 bulletScale = bullet.transform.localScale;

        go.transform.localScale = new Vector3(bulletScale.x * 3f, bulletScale.y * 3f, bulletScale.z);

        go.GetComponent<MeshRenderer>().material = bulletMaterial;
        go.GetComponent<TrailRenderer>().material = bulletMaterial;
        go.GetComponent<Ammo>().ownerId = netId;
        
        go.transform.parent = _bulletSpawn.transform;
        go.tag = "HeavyLaser";

        NetworkServer.Spawn(go);
    }

    /// <summary>
    /// Augmente la cadence de tir pour le power-up Mitraillette
    /// et désactive tout les autres power-up de tir
    /// </summary>
    [Server]
    public void PowerUpNewShootRate( )
    {
        // on ne peut avoir qu'un power-up de tir à la fois
        ResetPowerUps();
        _mitraillette = true;
        shootRate = _mitrailletteShootRate;
        StartCoroutine(TimerMitraillette());
    }

    /// <summary>
    /// Augmente la cadence de tir pour le power-up Akimbo
    /// et désactive tout les autres power-up de tir
    /// </summary>
    [Server]
    public void PowerUpAkimbo( )
    {
        ResetPowerUps();
        _akimbo = true;
        shootRate = _refShootRate;
        StartCoroutine(TimerAkimbo());
    }

    /// <summary>
    /// Augmente la cadence de tir pour le power-up Bazooka
    /// et désactive tout les autres power-up de tir
    /// </summary>
    [Server]
    public void PowerUpBazooka( )
    {
        ResetPowerUps();
        _bazooka = true;
        shootRate = _bazookaShootRate;
        StartCoroutine(TimerBazooka());
    }

    /// <summary>
    /// Augmente la cadence de tir pour le power-up Homing Missile
    /// et désactive tout les autres power-up de tir
    /// </summary>
    [Server]
    public void PowerUpHomingMissile()
    {
        ResetPowerUps();
        _homing = true;
        shootRate = _homingShootRate;
        StartCoroutine(TimerHomingMissile());
    }

    /// <summary>
    /// Augmente la cadence de tir pour le power-up Heavy laser
    /// et désactive tout les autres power-up de tir
    /// </summary>
    [Server]
    public void PowerUpHeavyLaser( )
    {
        ResetPowerUps();
        _heavy = true;
        shootRate = _heavyLaserShootRate;
        StartCoroutine(TimerHeavyLaser());
    }

    /// <summary>
    /// Fonction attendant 10 secondes avant de
    /// désactiver le power-up Akimbo
    /// </summary>
    [Server]
    private IEnumerator TimerAkimbo( )
    {
        // TODO: change value
        yield return new WaitForSeconds(10);

        _akimbo = false;
        shootRate = _refShootRate;
    }

    /// <summary>
    /// Fonction attendant 10 secondes avant de
    /// désactiver le power-up _mitraillette
    /// </summary>
    [Server]
    private IEnumerator TimerMitraillette( )
    {
        // TODO: change value
        yield return new WaitForSeconds(10);

        _mitraillette = false;
        shootRate = _refShootRate;
    }

    /// <summary>
    /// Fonction attendant 30 secondes avant de
    /// désactiver le power-up _bazooka
    /// </summary>
    [Server]
    private IEnumerator TimerBazooka( )
    {
        // TODO: change value
        yield return new WaitForSeconds(30);

        _bazooka = false;
        shootRate = _refShootRate;
    }

    /// <summary>
    /// Fonction attendant 30 secondes avant de
    /// désactiver le power-up _homing missile
    /// </summary>
    [Server]
    private IEnumerator TimerHomingMissile( )
    {
        // TODO: change value
        yield return new WaitForSeconds(30);
        
        _homing = false;
        shootRate = _refShootRate;
    }

    /// <summary>
    /// Fonction attendant 15 secondes avant de
    /// désactiver le power-up _heavy laser
    /// </summary>
    [Server]
    private IEnumerator TimerHeavyLaser( )
    {
        // TODO: change value
        yield return new WaitForSeconds(15);

        _heavy = false;
        shootRate = _refShootRate;
    }

    /// <summary>
    /// Supprime tout les tirs spéciaux liés aux power-up, utilisé quand un joueur meurt
    /// </summary>
    [Server]
    public void ResetPowerUps( )
    {
        StopAllCoroutines();
        _akimbo = _bazooka = _mitraillette = _homing = _heavy = false;
        shootRate = _refShootRate;
    }
}
