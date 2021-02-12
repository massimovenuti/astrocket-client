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
        float a = UI.GetComponent<RectTransform>().rect.width;
        float b = UI.GetComponent<RectTransform>().rect.height;
        a = a - a/8;
        b = b - b/8;

        rot += (rot > 0f) ? 0f :  360f;
        rotRadian += (rot > 0f) ? 0f : 2f * Mathf.PI; 

        float rectAtan = Mathf.Atan2(b,a);
        float tanTheta = Mathf.Tan(rotRadian);
        int region;
                
        if ((rotRadian > -rectAtan) && (rotRadian <= rectAtan))
            region = 1;
        else if ((rotRadian > rectAtan) && (rotRadian <= (Mathf.PI - rectAtan)))
            region = 2;
        else if ((rotRadian > (Mathf.PI - rectAtan)) || (rotRadian <= -(Mathf.PI - rectAtan)))
            region = 3;
        else
            region = 4;

        int xFactor = 1;
        int yFactor = 1;

        if(region == 1 || region == 2) {
            xFactor = -1;
            yFactor = -1;  
        }

        if ((region == 1) || (region == 3)) {
            screenPos.x += xFactor * (a / 2);
            screenPos.y += yFactor * (a / 2) * tanTheta;
        } else {
            screenPos.x += xFactor * (b / (2 * tanTheta));
            screenPos.y += yFactor * (b/  2);
        }

        indicator.GetComponent<RectTransform>().anchoredPosition = screenPos;
    }
}
