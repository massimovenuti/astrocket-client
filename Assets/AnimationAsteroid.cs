using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAsteroid : MonoBehaviour
{
    private Animator _asteroidController;
    // Start is called before the first frame update
    void Start()
    {
        float ran = Random.value;
        _asteroidController.SetFloat("FloatSpin", ran);
        //Debug.Log(ran);
    }

    private void Awake( )
    {
        _asteroidController = this.GetComponent<Animator>();
        Random.InitState(Time.frameCount);        
    }
}
