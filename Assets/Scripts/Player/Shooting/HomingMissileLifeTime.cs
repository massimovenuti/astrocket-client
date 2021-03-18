using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissileLifeTime : MonoBehaviour
{

    public GameObject BulletStorage;
    public GameObject PlayerParent;
    public GameObject RealPlayer;

    private GameObject target;
    public Transform targetTransform;
    private Rigidbody _rb;
    private float _rotateMissileSpeed = 5f;
    private float _force = 20f;
    Vector3 direction;

    GameObject[] tblTargets;

    // TODO: change value
    float threshold = 15f;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start( )
    {
        // on récupère le joueur ayant tiré le missile
        BulletStorage = this.transform.parent.gameObject;
        PlayerParent = BulletStorage.gameObject.transform.parent.gameObject;
        RealPlayer = PlayerParent.gameObject.transform.Find("Player").gameObject;

        _rb = GetComponent<Rigidbody>();

        // on récupère la cible la plus proche du joueur
        target = ClosestEnenmy();

        if (target != null)
            targetTransform = target.GetComponent<Transform>();

        Destroy(gameObject, 5);
    }

    void FixedUpdate( )
    {
        // si on a une cible proche du joueur, on modifie
        // la trajectoire du missible
        if (target != null && IsCloseEnemy(target))
        {
            direction = targetTransform.position - _rb.position;
            direction.Normalize();
            Vector3 rotationAmount = Vector3.Cross(transform.up, direction);
            _rb.angularVelocity = rotationAmount * _rotateMissileSpeed;
            _rb.velocity = transform.up * _force;
        }
        // sinon il va tout droit
        else
        {
            _rb.velocity = transform.up * _force;
        }

        
    }

    /// <summary>
    /// Trouve l'ennemi le plus proche et devient la cible du homing missile
    /// </summary>
    private GameObject ClosestEnenmy( )
    {
        tblTargets = GameObject.FindGameObjectsWithTag("Player");

        if ((tblTargets.Length == 0) || (tblTargets.Length == 1 && tblTargets[0] == RealPlayer))
            return null;

        GameObject Truetarget = null;

        Vector3 targetDistance;

        // pour éviter que le joueur ne se cible lui-même
        if (tblTargets[0] == RealPlayer)
            targetDistance = tblTargets[1].transform.position;
        else
            targetDistance = tblTargets[0].transform.position;

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

        return Truetarget;
    }

    /// <summary>
    /// Test s'il y a des ennemis proches.
    /// Si la distance entre un ennemi et le vaiseau est supérieur au seuil, alors il n'est pas proche
    /// </summary>
    private bool IsCloseEnemy(GameObject Closest)
    {
        if (Vector3.Distance(Closest.transform.position, RealPlayer.transform.position) <= threshold)
            return true;

        return false;
    }

}
