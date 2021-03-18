using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    private Dictionary<string, long> _scores = new Dictionary<string, long>(8);
    public Dictionary<string, long> Scores { get => _scores; }


    private static ScoreManager _instance = null;
    public static ScoreManager Instance
    {
        get => _instance;
    }

    private void OnEnable( )
    {
        if( _instance == null)
        {
            _instance = this;
        } else
        {
            Destroy(this);
        }
    }

    public void AddPlayer(string token)
    {
        if (_scores.ContainsKey(token))
            return;
        _scores[token] = 0;
    }
    public void AddScore(string token, long score)
    {
        if (_scores.ContainsKey(token) == false)
            AddPlayer(token);

        _scores[token] += score;
    }
}
