using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReturnToBattle : MonoBehaviour
{
    public GameObject player;

    public static ReturnToBattle instance;
    public GameObject UI;
    
    private Image background;
    private Image indicator;
    private TextMeshProUGUI timerText;

    private Color initialAlpha;
    private Color tmpAlpha;
    private float AlphaFactor = 15f;

    private float initialTimer = 10f;
    private float timer;
    private bool trigger = false;

    private void Awake()
    {
        instance = this;
        timerText = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
        background = GameObject.Find("Background").GetComponent<Image>();
        indicator = GameObject.Find("CenterIndicator").GetComponent<Image>();
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

            if(timer <= 10f)
                timer += Time.deltaTime;
        }

        CalcIndicator();
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

    private void CalcIndicator()
    {
        float rotRadian = Mathf.Atan2(player.transform.position.z, player.transform.position.x);
        float rot = Mathf.Rad2Deg * rotRadian;
        indicator.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, rot + 90f);
        
        Vector2 screenPos = Vector2.zero;

        float a = Screen.width;
        float b = Screen.height;
        
        rot += (rot > 0f) ? 0f :  360f;
        rotRadian += (rot > 0f) ? 0f : 2 * Mathf.PI; 

        if(rot > 45f && rot <= 135f) {
            screenPos = new Vector2(b * Mathf.Cos(rotRadian)/2 * Mathf.Sin(rotRadian), b/2);
        } else if(rot > 135f && rot <= 225f) {
            screenPos = new Vector2(-a/2, a * Mathf.Sin(rotRadian)/2 * Mathf.Cos(rotRadian));
        } else if(rot > 225f && rot <= 315f) {
            screenPos = new Vector2(-b * -Mathf.Cos(rotRadian)/2 * Mathf.Sin(rotRadian), b/2);
        } else {
            screenPos = new Vector2(a/2, a * Mathf.Sin(rotRadian)/2 * Mathf.Cos(rotRadian));
        }
                
        indicator.GetComponent<RectTransform>().anchoredPosition = screenPos;
    }
}
