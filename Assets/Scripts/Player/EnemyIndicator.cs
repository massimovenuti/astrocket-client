using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Mirror;

public class EnemyIndicator : NetworkBehaviour
{
    [SerializeField] GameObject _UI;

    private Image _enemyIndicator;
    private Camera _playerCamera;

    private List<GameObject> _enemies = new List<GameObject>();
    private GameObject _lastPlayer = null;

    private void Awake()
    {
        _enemyIndicator = _UI.FindObjectByName("EnemyIndicator").GetComponent<Image>();
        _playerCamera = gameObject.FindObjectByName("Camera").GetComponent<Camera>();
        _enemyIndicator.enabled = false;
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            GameObject closest = getClosestEnemy();
            if (closest != null)
            {
                if(closest != _lastPlayer)
                {
                    Color c = closest.GetComponent<PlayerInfo>().color;
                    _enemyIndicator.color = c;
                    _lastPlayer = closest;
                }

                Vector3 viewPos = _playerCamera.WorldToViewportPoint(closest.transform.position);
                if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
                {
                    _enemyIndicator.enabled = false;
                }
                else
                {
                    _enemyIndicator.enabled = true;
                    PlayerReturnToBattle.CalcIndicator(_UI, gameObject.transform.position, closest.transform.position, _enemyIndicator);
                }
            }
            else
            {
                _enemyIndicator.enabled = false;
            }
        }
    }

    private GameObject getClosestEnemy( ) 
    {
        _enemies.Clear();
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players)
        {
            if (player != gameObject) // removing own player
                _enemies.Add(player);
        }

        GameObject closest = null;
        float minDistance = Mathf.Infinity;
        Vector3 pos = gameObject.transform.position;
        foreach(GameObject enemy in _enemies)
        {
            Vector3 diff = enemy.transform.position - pos;
            float distance = diff.sqrMagnitude;
            if (distance < minDistance)
            {
                closest = enemy;
                minDistance = distance;
            }
        }
        return closest;
    }
}
