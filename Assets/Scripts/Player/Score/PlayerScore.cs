using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerScore : NetworkBehaviour
{
    [SerializeField]
    private int pointsKill;
    [SerializeField]
    private int pointsAsteroids;
    [SerializeField]
    private int pointsDeaths;
    [SerializeField]
    private int pointsPowerUps;

    [SyncVar]
    private int nbPoints;
    [SyncVar]
    private int nbKills;
    [SyncVar]
    private int nbAsteroids;
    [SyncVar]
    private int nbDeaths;
    [SyncVar]
    private int nbPowerUps;

    [Server]
    public void addKill( )
    {
        nbKills++;
        nbPoints += pointsKill;
    }

    [Server]
    public void addAsteroid( )
    {
        nbAsteroids++;
        nbPoints += pointsAsteroids;
    }

    [Server]
    public void addDeath( )
    {
        nbDeaths++;
        nbPoints += pointsDeaths;
        nbPoints = (nbPoints < 0) ? 0 : nbPoints;
    }

    [Server]
    public void addPowerUP()
    {
        nbPowerUps++;
        nbPoints += pointsPowerUps;
    }
}
