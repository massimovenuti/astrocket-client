using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissileLifeTime : MonoBehaviour
{

    public GameObject BulletStorage;
    public GameObject PlayerParent;
    public GameObject RealPlayer;

    public GameObject target;
    public Transform targetTransform;
    private Rigidbody _rb;
    private float _rotateMissileSpeed = 5f;
    private float _force = 20f;
    Vector3 direction;

    GameObject[] tblTargets;


    // Start is called before the first frame update
    void Start()
    {
        BulletStorage = this.transform.parent.gameObject;
        PlayerParent = BulletStorage.gameObject.transform.parent.gameObject;
        RealPlayer = PlayerParent.gameObject.transform.Find("Player").gameObject;

        //TODO CHANGE TAG
        tblTargets = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject Truetarget = null;
        

        Vector3 targetDistance = tblTargets[0].transform.position;

        foreach(GameObject targets in tblTargets)
        {
            Vector3 tmp = targets.transform.position;
            
            if (CalculateDistance(tmp, RealPlayer.transform.position) <= CalculateDistance(targetDistance, RealPlayer.transform.position))
            {
                targetDistance = tmp;
                Truetarget = targets;
            }
        }

        target = Truetarget;

        print(target.name);

        targetTransform = target.GetComponent<Transform>();
        _rb = GetComponent<Rigidbody>();

        //A voir pour le destroy
        Destroy(gameObject, 15);
    }

    void FixedUpdate()
    {
        direction = targetTransform.position-_rb.position;
        direction.Normalize();
        Vector3 rotationAmount = Vector3.Cross(transform.up, direction);
        _rb.angularVelocity = rotationAmount * _rotateMissileSpeed;
        _rb.velocity = transform.up * _force;
    }


    private float CalculateDistance(Vector3 a, Vector3 b)
    {
        return Mathf.Sqrt(Mathf.Pow(b.x - a.x,2) + Mathf.Pow(b.y - a.y,2));
    }

}
