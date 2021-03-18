using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <author>
/// Joséphine Largent
/// </author>

/// <summary>
/// Affichage du score sur l'écran
/// </summary>

public class PlayerScore : MonoBehaviour
{
    private ScoreObserver observer;
    Text textScore;

    /// <summary>
    /// Initialisation des objets
    /// </summary>
    void Start()
    {
        observer = GameObject.Find("PlayerFinal").GetComponent<ScoreObserver>();
        textScore = GetComponent<Text>();
    }

    /// <summary>
    /// Màj de l'UI score
    /// </summary>
    void Update()
    {
        textScore.text = "Score: " + observer.Score;
    }
}
