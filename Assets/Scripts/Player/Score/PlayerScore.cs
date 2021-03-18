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
    private Text textScore;
    private ScoreManager observer;
    private string _mockPlayerId = "id";

    /// <summary>
    /// Initialisation des objets
    /// </summary>
    void Start()
    {
        observer = ScoreManager.Instance;
        observer.AddPlayer(_mockPlayerId);
        textScore = GetComponent<Text>();
    }

    /// <summary>
    /// Màj de l'UI score
    /// </summary>
    void Update()
    {
        textScore.text = "Score: " + observer.Scores[_mockPlayerId];
    }
}
