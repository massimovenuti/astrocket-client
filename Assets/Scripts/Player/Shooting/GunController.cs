using System.Linq;
using UnityEngine;
using System.Collections;
using Mirror;
using TMPro;

public class GunController : NetworkBehaviour
{
    public GameObject bullet;

    [SyncVar]
    public float shootRate = 0.2f;
    
    public Material bulletMaterial;
    public float shootForce = 3000f;
    public string ShootingFrom = "Barrel";
    public string BulletStorageTagName = "BulletStorage";

    [SyncVar]
    private bool _akimbo, _bazooka;

    private float _refShootRate, _bazookaShootRate;

    private InputManager _inp;

    private GameObject _barrel, _barrelAkimbo1, _barrelAkimbo2, _bulletSpawn;

    public GameObject rocket;

    private float _lastShootingTimeRef;

    [SyncVar]
    public bool canShoot;

    private ShootingIndicator _shootingIndicator;

    [SyncVar]
    private float _timer, _refTimer;

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

        _inp = InputManager.InputManagerInst;
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
            _akimbo = _bazooka = false;
        }

        _refShootRate = shootRate;
        _bazookaShootRate = shootRate * 2;

        _shootingIndicator = GetComponent<ShootingIndicator>();
    }

    private void Update( )
    {
        if (isLocalPlayer)
        {
            DisplayShooting();

            if (Time.time > _lastShootingTimeRef && _inp.IsShooting())
            {
                CmdShoot();
                _lastShootingTimeRef = Time.time + shootRate;
            }
        }

        if (!canShoot || _akimbo || _bazooka)
        {
            if (isServer)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0)
                {
                    ResetShooting();
                }
            }
            else if(isLocalPlayer)
            {
                _shootingIndicator.DisplayTimer(_refTimer, _timer);
            }
        } 
        else if (isLocalPlayer)
        {
            _shootingIndicator.DisplayTimer(_refTimer, 0.0f);
        }
    }

    private void DisplayShooting()
    {
        if (!canShoot)
        {
            _shootingIndicator.DisplayCantShoot();
        }
        else if (_akimbo)
        {
            _shootingIndicator.DisplayAkimbo();
        }
        else if (_bazooka)
        {
            _shootingIndicator.DisplayBazooka();
        }
        else
        {
            _shootingIndicator.DisplaySingleFire();
        }
    }

    [Command]
    private void CmdShoot()
    {
        if (!canShoot)
            return;

        if (_akimbo)
        {
            ShootAkimbo();
        }
        else if (_bazooka)
        {
            ShootBazooka();
        }
        else
        {
            Shoot();
        }
    }

    /// <summary>
    /// Fonction de tir de base (instancie un tir)
    /// </summary>
    [Server]
    private void Shoot( )
    {
        GameObject go = (GameObject)Instantiate(bullet, _barrel.transform.position, _barrel.transform.rotation);
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

        go1.GetComponent<Ammo>().ownerId = netId;
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
    /// Augmente la cadence de tir pour le power-up Akimbo
    /// et désactive tout les autres power-up de tir
    /// </summary>
    [Server]
    public void PowerUpAkimbo( )
    {
        ResetShooting();
        _refTimer = 15;
        _timer = _refTimer;
        _akimbo = true;
        shootRate = _refShootRate;
    }

    /// <summary>
    /// Augmente la cadence de tir pour le power-up Bazooka
    /// et désactive tout les autres power-up de tir
    /// </summary>
    [Server]
    public void PowerUpBazooka( )
    {
        ResetShooting();
        _refTimer = 15;
        _timer = _refTimer;
        _bazooka = true;
        shootRate = _bazookaShootRate;
    }

    [Server]
    public void DisableShoot()
    {
        _refTimer = 2;
        _timer = _refTimer;
        canShoot = false;
    }

    /// <summary>
    /// Supprime tout les tirs spéciaux liés aux power-up, utilisé quand un joueur meurt
    /// </summary>
    [Server]
    public void ResetShooting( )
    {
        _akimbo = _bazooka = false;
        canShoot = true;
        shootRate = _refShootRate;
    }
}
