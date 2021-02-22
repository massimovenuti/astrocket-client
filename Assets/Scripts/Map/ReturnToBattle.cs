using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReturnToBattle : MonoBehaviour
{
    public string RTBManagerName = "ReturnToBattleManager";

    private GameObject _UI;
    private GameObject _player;

    private Image _indicator;
    private Image _background;
    private TextMeshProUGUI _timerText;

    private Color _tmpAlpha;
    private Color _initialAlpha;

    private float _timer;
    private readonly float _alphaFactor = 15f;
    private readonly float _initialTimer = 10f;

    private bool _trigger = false;

    private void Awake( )
    {
        _player = GameObject.FindGameObjectsWithTag("Player").First();

        _UI = GameObject.Find(RTBManagerName);
        Debug.Assert(_UI != null);
        // We can use public strings here if necessary (I find it overkill though)
        _background = _UI.FindObjectByName("Background").GetComponent<Image>();
        _indicator = _UI.FindObjectByName("CenterIndicator").GetComponent<Image>();
        _timerText = _UI.FindObjectByName("TimerText").GetComponent<TextMeshProUGUI>();
    }

    private void Start( )
    {
        _timer = _initialTimer;
        _initialAlpha = _background.color;
        _tmpAlpha = _initialAlpha;
    }

    private void Update( )
    {
        if (_trigger)
        {
            _timerText.text = _timer.ToString("F1");

            if (_timer <= 0 && _trigger)
            {
                PlayerHealth ph = _player.GetComponent<PlayerHealth>();
                ph.playerHealth.Damage(ph.playerHealth.GetHealth());
                _trigger = false;
                _timer = 10f;

                Debug.Log("Dead");
            }
            else
            {
                _timer -= Time.deltaTime;
                _tmpAlpha.a = _initialAlpha.a + (_initialTimer - _timer) / _alphaFactor;
                _background.color = _tmpAlpha;
            }

            CalcIndicator();
        }
        else
        {
            if (_timer <= 10f)
                _timer += Time.deltaTime;
        }
    }

    public void EnterArea( )
    {
        _trigger = false;
        _UI.SetActive(_trigger);
        _background.color = _initialAlpha;
    }

    public void ExitArea( )
    {
        _trigger = true;
        _UI.SetActive(_trigger);
    }

    private void CalcIndicator( )
    {
        float rotRadian = Mathf.Atan2(_player.transform.position.z, _player.transform.position.x);
        float rot = Mathf.Rad2Deg * rotRadian;
        _indicator.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, rot + 90f);

        Vector2 screenPos = Vector2.zero;
        float a = _UI.GetComponent<RectTransform>().rect.width;
        float b = _UI.GetComponent<RectTransform>().rect.height;
        a -= a / 8;
        b -= b / 8;

        rot += (rot > 0f) ? 0f : 360f;
        rotRadian += (rot > 0f) ? 0f : 2f * Mathf.PI;

        float rectAtan = Mathf.Atan2(b, a);
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

        if (region == 1 || region == 2)
        {
            xFactor = -1;
            yFactor = -1;
        }

        if ((region == 1) || (region == 3))
        {
            screenPos.x += xFactor * (a / 2);
            screenPos.y += yFactor * (a / 2) * tanTheta;
        }
        else
        {
            screenPos.x += xFactor * (b / (2 * tanTheta));
            screenPos.y += yFactor * (b / 2);
        }

        _indicator.GetComponent<RectTransform>().anchoredPosition = screenPos;
    }
}
