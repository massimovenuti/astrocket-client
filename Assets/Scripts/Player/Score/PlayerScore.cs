using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerScore : NetworkBehaviour
{
    public short pointsKill = 0;
    public short pointsAsteroids = 0;
    public short pointsDeaths = 0;
    public short pointsPowerUps = 0;


    [SyncVar(hook = "updatePointsUi")]
    public short nbPoints = 0;
    [SyncVar(hook = "updateKillsUi")]
    public ushort nbKills = 0;
    [SyncVar(hook = "updateAsteroidsUi")]
    public ushort nbAsteroids = 0;
    [SyncVar(hook = "updateDeathsUi")]
    public ushort nbDeaths = 0;
    [SyncVar(hook = "updatePuUi")]
    public ushort nbPowerUps = 0;

    private ScoreTabManager scoreTabManager;


    [ClientCallback]
    private void Start( )
    {
        scoreTabManager = GameObject.Find("ScoreCanvas").GetComponent<ScoreTabManager>();
    }

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
        nbPoints = (nbPoints < (short)0) ? (short)0 : nbPoints;
    }

    [Server]
    public void addPowerUP()
    {
        nbPowerUps++;
        nbPoints += pointsPowerUps;
    }

    [Client]
    void updatePointsUi(short oldValue, short newValue)
    {
        scoreTabManager.updateValue("Score", GetComponent<PlayerInfo>().playerName, newValue);
    }

    [Client]
    void updateKillsUi(ushort oldValue, ushort newValue)
    {
        scoreTabManager.updateValue("Kills", GetComponent<PlayerInfo>().playerName, newValue);
    }

    [Client]
    void updateDeathsUi(ushort oldValue, ushort newValue)
    {
        scoreTabManager.updateValue("Deaths", GetComponent<PlayerInfo>().playerName, newValue);
    }

    [Client]
    void updateAsteroidsUi(ushort oldValue, ushort newValue)
    {
        scoreTabManager.updateValue("Asteroids", GetComponent<PlayerInfo>().playerName, newValue);
    }

    [Client]
    void updatePuUi(ushort oldValue, ushort newValue)
    {
        scoreTabManager.updateValue("Power-ups", GetComponent<PlayerInfo>().playerName, newValue);
    }

    /*
    [ClientCallback]
    private void OnDestroy( )
    {
        if (scoreTabManager != null)
        {
            scoreTabManager.rmLigne(GetComponent<PlayerInfo>().playerName);
        }
    }
    */
}
