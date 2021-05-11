using Mirror;
using TMPro;
using UnityEngine;

public class PlayerScore : NetworkBehaviour
{
    public short pointsKill = 100;
    public short pointsAsteroids = 10;
    public short pointsDeaths = -35;
    public short pointsPowerUps = 25;


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

    [SyncVar(hook = "updateRankUi")]
    public ushort rank = 0;

    private ScoreTabManager scoreTabManager;

    private TextMeshProUGUI _rankText;

    public override void OnStartClient( )
    {
        scoreTabManager = GameObject.Find("ScoreCanvas").GetComponent<ScoreTabManager>();
        _rankText = GameObject.Find("Rank").GetComponent<TextMeshProUGUI>();
        if (isLocalPlayer)
        {
            updateRankUi(0, rank);
        }
    }

    [Server]
    public void addKill( )
    {
        nbKills++;
        nbPoints += pointsKill;
        UpdateRank();
    }

    [Server]
    public void addAsteroid( )
    {
        nbAsteroids++;
        nbPoints += pointsAsteroids;
        UpdateRank();
    }

    [Server]
    public void addDeath( )
    {
        nbDeaths++;
        nbPoints += pointsDeaths;
        nbPoints = (nbPoints < (short)0) ? (short)0 : nbPoints;
        UpdateRank();
    }

    [Server]
    public void addPowerUP()
    {
        nbPowerUps++;
        nbPoints += pointsPowerUps;
        UpdateRank();
    }

    [Server]
    private void UpdateRank()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
        {
            PlayerScore score = go.GetComponent<PlayerScore>();
            if (score.rank < rank && score.nbPoints < nbPoints)
            {
                score.rank++;
                rank--;
            }
            else if (score.rank > rank && score.nbPoints > nbPoints)
            {
                score.rank--;
                rank++;
            }
        }
    }

    [Client]
    void updatePointsUi(short _, short newValue)
    {
        scoreTabManager.updateValue("Score", GetComponent<PlayerInfo>().playerName, newValue);
    }

    [Client]
    void updateKillsUi(ushort _, ushort newValue)
    {
        scoreTabManager.updateValue("Kills", GetComponent<PlayerInfo>().playerName, newValue);
    }

    [Client]
    void updateDeathsUi(ushort _, ushort newValue)
    {
        scoreTabManager.updateValue("Deaths", GetComponent<PlayerInfo>().playerName, newValue);
    }

    [Client]
    void updateAsteroidsUi(ushort _, ushort newValue)
    {
        scoreTabManager.updateValue("Asteroids", GetComponent<PlayerInfo>().playerName, newValue);
    }

    [Client]
    void updatePuUi(ushort _, ushort newValue)
    {
        scoreTabManager.updateValue("Power-ups", GetComponent<PlayerInfo>().playerName, newValue);
    }

    [Client]
    void updateRankUi(ushort _, ushort newValue)
    {
        if (isLocalPlayer && _rankText != null)
        {
            _rankText.text = "#" + newValue.ToString();
        }
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
