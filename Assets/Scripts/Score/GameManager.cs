using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author>
/// Joséphine Largent
/// </author>

/// <summary>
/// Maître des évènements (singleton)
/// permet aux joueurs de s'enregistrer et de gérer des évènements de points
/// </summary>

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance { 
        get {
            if (instance == null)
            {
                // creates a game object and attached this script to it
                instance = new GameObject("GameManager").AddComponent<GameManager>(); 
            }
            return instance;
        } 
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    // Update is called once per frame
    public void IncreaseScore(string playerId, long points)
    {
        
    }
}
