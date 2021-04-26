using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class ScoreTabManager : MonoBehaviour
{
    public GameObject pCell;
    public GameObject dCell;
    public void addLigne(GameObject player)
    {
        string name = $"player_{player.GetComponent<NetworkIdentity>().netId}";
        PlayerScore score = player.GetComponent<PlayerScore>();

        GameObject playerCell = Instantiate(pCell, transform.Find("Board/Players").transform);
        playerCell.name = name;
        playerCell.GetComponentInChildren<TMP_Text>().text = name;
        playerCell.GetComponentInChildren<Image>().color = player.GetComponent<PlayerSetup>().playerColor;

        GameObject scoreCell = Instantiate(dCell, transform.Find("Board/Score").transform);
        scoreCell.name = name;
        scoreCell.GetComponentInChildren<TMP_Text>().text = score.nbPoints.ToString();
        
        GameObject killCell = Instantiate(dCell, transform.Find("Board/Kills").transform);
        killCell.name = name;
        killCell.GetComponentInChildren<TMP_Text>().text = score.nbKills.ToString();
        
        GameObject deathCell = Instantiate(dCell, transform.Find("Board/Deaths").transform);
        deathCell.name = name;
        deathCell.GetComponentInChildren<TMP_Text>().text = score.nbDeaths.ToString();
        
        GameObject asteroidsCell = Instantiate(dCell, transform.Find("Board/Asteroids").transform);
        asteroidsCell.name = name;
        asteroidsCell.GetComponentInChildren<TMP_Text>().text = score.nbAsteroids.ToString();
        
        GameObject puCell = Instantiate(dCell, transform.Find("Board/Power-ups").transform);
        puCell.name = name;
        puCell.GetComponentInChildren<TMP_Text>().text = score.nbPowerUps.ToString();
    }

    public void updateValue(string valueName, string playerName, int newVal)
    {
        transform.Find($"Board/{valueName}/{playerName}").GetComponentInChildren<TMP_Text>().text = newVal.ToString();
    }

    public void rmLigne(string name)
    {
        Destroy(transform.Find($"Board/Players/{name}").gameObject);
        Destroy(transform.Find($"Board/Score/{name}").gameObject);
        Destroy(transform.Find($"Board/Kills/{name}").gameObject);
        Destroy(transform.Find($"Board/Deaths/{name}").gameObject);
        Destroy(transform.Find($"Board/Asteroids/{name}").gameObject);
        Destroy(transform.Find($"Board/Power-ups/{name}").gameObject);
    }
}
