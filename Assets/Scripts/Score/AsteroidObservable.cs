using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <author>
/// Joséphine Largent
/// </author>

/// <summary>
/// Score pour un astéroide
/// </summary>

public class AsteroidObservable : MonoBehaviour
{
    protected ScoreObserver observer;
    [SerializeField]
    protected uint Score;

    /// <summary>
    /// Initalisation de la valeur de défaut d'un astéroide spécial
    /// </summary>
    void Awake()
    {
        Score = 100;
    }

    /// <summary>
    /// Initialisation de l'objet
    /// </summary>
    void Start( )
    {
        observer = GameObject.Find("Player").GetComponent<ScoreObserver>();
    }

    // Update is called once per frame
    void Update()
    {
        /// animation de l'astéroide ?
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Debug.Log("hit asteroid");
            observer.addScore(Score);
        }
    }
}
