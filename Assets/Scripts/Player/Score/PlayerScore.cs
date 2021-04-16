using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerScore : NetworkBehaviour
{
    [SerializeField]
    private short pointsKill = 0;
    [SerializeField]
    private short pointsAsteroids = 0;
    [SerializeField]
    private short pointsDeaths = 0;
    [SerializeField]
    private short pointsPowerUps = 0;

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

    [SerializeField]
    private Canvas scoreCanvas;

    private CanvasGroup scoreCanvasGroup;
    private ScoreTabManager scoreTabManager;

    private void Awake( )
    {
        scoreCanvasGroup = scoreCanvas.GetComponent<CanvasGroup>();
        scoreCanvasGroup.alpha = 0f;
    }

    [ClientCallback]
    private void Start( )
    {
        foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (p.GetComponent<NetworkBehaviour>().isLocalPlayer)
            {
                scoreTabManager = p.transform.Find("ScoreCanvas").GetComponent<ScoreTabManager>();
                break;
            }
        }
    }

    private void Update( )
    {
        if (isLocalPlayer)
        {
            if (Input.GetKey(KeyCode.Tab))
            {
                scoreCanvasGroup.alpha = 1f;
                scoreCanvasGroup.blocksRaycasts = true;
            }
            else
            {
                scoreCanvasGroup.alpha = 0f;
                scoreCanvasGroup.blocksRaycasts = false;
            }                
        }
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
        scoreTabManager.updateValue("Score", $"player_{netId}", newValue);
    }

    [Client]
    void updateKillsUi(ushort oldValue, ushort newValue)
    {
        scoreTabManager.updateValue("Kills", $"player_{netId}", newValue);
    }

    [Client]
    void updateDeathsUi(ushort oldValue, ushort newValue)
    {
        scoreTabManager.updateValue("Deaths", $"player_{netId}", newValue);
    }

    [Client]
    void updateAsteroidsUi(ushort oldValue, ushort newValue)
    {
        scoreTabManager.updateValue("Asteroids", $"player_{netId}", newValue);
    }

    [Client]
    void updatePuUi(ushort oldValue, ushort newValue)
    {
        scoreTabManager.updateValue("Power-ups", $"player_{netId}", newValue);
    }

    [ClientCallback]
    private void OnDestroy( )
    {
        scoreTabManager.rmLigne($"player_{netId}");
    }
}
