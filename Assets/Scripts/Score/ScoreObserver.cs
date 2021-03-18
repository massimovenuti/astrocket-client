using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author>
/// Joséphine Largent
/// </author>

/// <summary>
/// Gestion du score avec usage de pattern observer
/// </summary>

public class ScoreObserver : MonoBehaviour
{
    [SerializeField]
    public uint Score = 0;

    public int idPlayer;

    // Start is called before the first frame update
    /// <summary>
    /// Initialisation du score
    /// </summary>
    void Start()
    {
        
    }

    /// <summary>
    /// Augmentation du score
    /// </summary>
    public void addScore(uint scoreVal)
    {
        Score += scoreVal;
    }

    /*
     public class ScoreObserver : MonoBehaviour
{
    [SerializeField]
    private Dictionary<string, long> Score = new Dictionary<string, long>(8); //fixed to 8 bc no more than 8 players

    // Start is called before the first frame update
    /// <summary>
    /// Initialisation du score
    /// </summary>
    void Start()
    {
        
    }

    /// <summary>
    /// Ajout d'un joueur
    /// </summary>
    public void addPlayer(string playerId)
    {
        Score[playerId] = 0;
    }

    /// <summary>
    /// Augmentation du score d'un joueur
    /// </summary>
    public void addScore(string playerId, long scoreVal)
    {
        Score[playerId] += scoreVal;
    }
}
     */
}
