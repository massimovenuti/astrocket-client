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

    //A MODIFIER POUR L'EQUILIBRAGE
    float treshold = 15f;


    // Start is called before the first frame update
    void Start( )
    {

        BulletStorage = this.transform.parent.gameObject;
        PlayerParent = BulletStorage.gameObject.transform.parent.gameObject;
        RealPlayer = PlayerParent.gameObject.transform.Find("Player").gameObject;

        _rb = GetComponent<Rigidbody>();


        target = ClosestEnenmy();

        if (target != null)
        {
            // DEBUG
            Debug.Log(target.name);

            targetTransform = target.GetComponent<Transform>();
        }

        Destroy(gameObject, 10);
    }

    void FixedUpdate( )
    {

        if (target != null && IsCloseEnemy(target))
        { 
            

            direction = targetTransform.position - _rb.position;
            direction.Normalize();
            Vector3 rotationAmount = Vector3.Cross(transform.up, direction);
            _rb.angularVelocity = rotationAmount * _rotateMissileSpeed;
            _rb.velocity = transform.up * _force;
        }
        else
        {
            _rb.velocity = transform.up * _force;
        }

        
    }

    private GameObject ClosestEnenmy( )
    {
        //TODO CHANGE TAG
        tblTargets = GameObject.FindGameObjectsWithTag("Enemy");

        if (tblTargets.Length == 0)
            return null;

        GameObject Truetarget = null;

        Vector3 targetDistance = tblTargets[0].transform.position;

        foreach (GameObject targets in tblTargets)
        {
            Vector3 tmp = targets.transform.position;

            if (Vector3.Distance(tmp, RealPlayer.transform.position) <= Vector3.Distance(targetDistance, RealPlayer.transform.position))
            {
                targetDistance = tmp;
                Truetarget = targets;
            }
        }

        return Truetarget;
    }

    private bool IsCloseEnemy(GameObject Closest)
    {
        if (Vector3.Distance(Closest.transform.position, RealPlayer.transform.position) <= treshold)
            return true;

        return false;
    }

}
