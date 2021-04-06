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

    [SyncVar(hook = "updatePointsUi")]
    public int nbPoints;
    [SyncVar(hook = "updateKillsUi")]
    public int nbKills;
    [SyncVar(hook = "updateAsteroidsUi")]
    public int nbAsteroids;
    [SyncVar(hook = "updateDeathsUi")]
    public int nbDeaths;
    [SyncVar(hook = "updatePuUi")]
    public int nbPowerUps;

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

    [Client]
    void updatePointsUi(int oldValue, int newValue)
    {
        GameObject canvas = GameObject.Find("ScoreCanvas");
        canvas.GetComponent<ScoreTabManager>().updateValue("Score", $"player_{netId}", newValue);
    }

    [Client]
    void updateKillsUi(int oldValue, int newValue)
    {
        GameObject canvas = GameObject.Find("ScoreCanvas");
        canvas.GetComponent<ScoreTabManager>().updateValue("Kills", $"player_{netId}", newValue);
    }

    [Client]
    void updateDeathsUi(int oldValue, int newValue)
    {
        GameObject canvas = GameObject.Find("ScoreCanvas");
        canvas.GetComponent<ScoreTabManager>().updateValue("Deaths", $"player_{netId}", newValue);
    }

    [Client]
    void updateAsteroidsUi(int oldValue, int newValue)
    {
        GameObject canvas = GameObject.Find("ScoreCanvas");
        canvas.GetComponent<ScoreTabManager>().updateValue("Asteroids", $"player_{netId}", newValue);
    }

    [Client]
    void updatePuUi(int oldValue, int newValue)
    {
        GameObject canvas = GameObject.Find("ScoreCanvas");
        canvas.GetComponent<ScoreTabManager>().updateValue("Power-ups", $"player_{netId}", newValue);
    }
}
