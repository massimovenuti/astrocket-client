using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HomingMissile : NetworkBehaviour
{
    public GameObject RealPlayer;

    private GameObject target;
    public Transform targetTransform;
    private Rigidbody _rb;
    private float _rotateMissileSpeed = 5f;
    private float _speed = 60f;
    Vector3 direction;

    GameObject[] tblTargets;

    // TODO: change value
    float threshold = 15f;

    private uint _ownerId;

    public override void OnStartServer( )
    {
        _ownerId = GetComponent<Ammo>().ownerId;

        _rb = GetComponent<Rigidbody>();

        // on récupère la cible la plus proche du joueur
        target = ClosestEnenmy();

        if (target != null)
        {
            targetTransform = target.GetComponent<Transform>();
        }
    }

    [ServerCallback]
    void FixedUpdate( )
    {
        // si on a une cible proche du joueur, on modifie
        // la trajectoire du missible
        if (target != null && IsCloseEnemy(target))
        {
            direction = targetTransform.position - _rb.position;
            direction.Normalize();
            Vector3 rotationAmount = Vector3.Cross(transform.forward, direction);
            _rb.angularVelocity = rotationAmount * _rotateMissileSpeed;
            _rb.velocity = transform.forward * _speed / 3;
        }
        else
        {
            _rb.velocity = transform.forward * _speed;
        }
    }

    /// <summary>
    /// Trouve l'ennemi le plus proche et devient la cible du homing missile
    /// </summary>
    [Server]
    private GameObject ClosestEnenmy( )
    {
        tblTargets = GameObject.FindGameObjectsWithTag("Player");

        //Debug.Log("tblTargets.Length : " + tblTargets.Length);

        if (tblTargets.Length <= 1)
        {
            return null;
        }

        GameObject Truetarget = null;

        Vector3 targetDistance;

        //Debug.Log("tblTargets[0].GetComponent<NetworkIdentity>().netId == _ownerId : " + (tblTargets[0].GetComponent<NetworkIdentity>().netId == _ownerId));
        // pour éviter que le joueur ne se cible lui-même
        if (tblTargets[0].GetComponent<NetworkIdentity>().netId == _ownerId)
        {
            targetDistance = tblTargets[1].transform.position;
        }
        else
        {
            targetDistance = tblTargets[0].transform.position;
        }

        foreach (GameObject targets in tblTargets)
        {
            if (targets.GetComponent<NetworkIdentity>().netId == _ownerId)
            {
                RealPlayer = targets;
                //Debug.Log("RealPlayer : " + RealPlayer);
            }
        }

        foreach (GameObject targets in tblTargets)
        {
            if (targets == RealPlayer)
                continue;

            Vector3 tmp = targets.transform.position;

            if (Vector3.Distance(tmp, RealPlayer.transform.position) <= Vector3.Distance(targetDistance, RealPlayer.transform.position))
            {
                targetDistance = tmp;
                Truetarget = targets;
            }
        }

        //Debug.Log("TrueTarget : " + Truetarget);
        return Truetarget;
    }

    /// <summary>
    /// Test s'il y a des ennemis proches.
    /// Si la distance entre un ennemi et le vaiseau est supérieur au seuil, alors il n'est pas proche
    /// </summary>
    [Server]
    private bool IsCloseEnemy(GameObject Closest)
    {
        if (Vector3.Distance(Closest.transform.position, RealPlayer.transform.position) <= threshold)
        {
            return true;
        }

        return false;
    }

}
