using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReturnToBattle : MonoBehaviour
{
    public static ReturnToBattle instance;
    public GameObject UI;
    
    private Image background;
    private TextMeshProUGUI timerText;

    private Color initialAlpha;
    private Color tmpAlpha;
    private float AlphaFactor = 15.0f;

    private float initialTimer = 10.0f;
    private float timer;
    private bool trigger = false;

    private void Awake()
    {
        instance = this;
        timerText = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
        background = GameObject.Find("Background").GetComponent<Image>();
    }

    private void Start()
    {
        timer = initialTimer;
        initialAlpha = background.color;
        tmpAlpha = initialAlpha;
    }

    private void Update()
    {
        if(trigger) {
            timerText.text = timer.ToString("F1");

            if(timer <= 0) {
                Debug.Log("Dead");
                trigger = false;

            } else {
                timer -= Time.deltaTime;
                tmpAlpha.a = initialAlpha.a + (initialTimer - timer)/AlphaFactor;
                background.color = tmpAlpha;
            }

        } else {

            if(timer <= 10.0f)
                timer += Time.deltaTime;
        }
    }

    public void EnterArea()
    {
        trigger = false;
        UI.SetActive(trigger);
        background.color = initialAlpha;
    }

    public void ExitArea()
    {
        trigger = true;
        UI.SetActive(trigger);
    }
}
