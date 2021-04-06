using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerReturnToBattle : NetworkBehaviour
{
    //public string RTBManagerName = "ReturnToBattleManager";

    [SerializeField] GameObject _UI;

    private Image _indicator;
    private Image _background;
    private TextMeshProUGUI _timerText;

    private Color _tmpAlpha;
    private Color _initialAlpha;

    [SyncVar(hook = "OnTimerChange")]
    private float _timer;

    [SyncVar(hook = "OnTriggerChange")]
    private bool _trigger = false;

    private readonly float _alphaFactor = 15f;
    private readonly float _initialTimer = 10f;


    private void Awake( )
    {
        //_UI = GameObject.Find(RTBManagerName);
        Debug.Assert(_UI != null);
        // We can use public strings here if necessary (I find it overkill though)
        _background = _UI.FindObjectByName("Background").GetComponent<Image>();
        _indicator = _UI.FindObjectByName("CenterIndicator").GetComponent<Image>();
        _timerText = _UI.FindObjectByName("TimerText").GetComponent<TextMeshProUGUI>();

        _initialAlpha = _background.color;
        _tmpAlpha = _initialAlpha;

        _UI.SetActive(false);
    }

    public override void OnStartServer( )
    {
        Reset();
    }

    [ClientCallback]
    private void OnEnable( )
    {
        if (isLocalPlayer)
        {
            _UI.SetActive(false);
            _background.color = _initialAlpha;
        }
    }

    [ClientCallback]
    private void OnTriggerChange(bool oldValue, bool newValue)
    {
        if (isLocalPlayer)
        {
            _UI.SetActive(newValue);
            if (newValue == false)
            {
                _background.color = _initialAlpha;
            }
        }
    }

    [ClientCallback]
    private void OnTimerChange(float oldValue, float newValue)
    {
        if (isLocalPlayer)
        {
            _timerText.text = newValue.ToString("F1");
            _tmpAlpha.a = _initialAlpha.a + (_initialTimer - newValue) / _alphaFactor;
            _background.color = _tmpAlpha;
        }
    }

    private void Update( )
    {
        if (_trigger)
        {
            if (isServer)
            {
                if (_timer <= 0 && _trigger)
                {
                    Reset();
                    GetComponent<PlayerSpawn>().Respawn();
                }
                else
                {
                    _timer = (_timer == 0) ? _timer : _timer - Time.deltaTime;
                }
            }
            else
            {
                CalcIndicator(Vector3.zero, _indicator); // target the center of the map
            }
        }
        else
        {
            if (isServer && _timer <= 10f)
            {
                _timer += Time.deltaTime;
                _timer = (_timer > 10f) ? 10f : _timer;
            }
        }
    }

    [Server]
    private void Reset( )
    {
        _trigger = false;
        _timer = _initialTimer;
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider intruder)
    {
        if (intruder.CompareTag("WarningBorder"))
        {
            _trigger = false;
        }
    }

    [ServerCallback]
    private void OnTriggerExit(Collider intruder)
    {
        if (intruder.CompareTag("WarningBorder"))
        {
            _trigger = true;
        }
        if (intruder.CompareTag("AbsoluteBorder"))
        {
            Reset();
            GetComponent<PlayerSpawn>().Respawn();
        }
    }

    [Client]
    private void CalcIndicator(Vector3 target, Image arrow)
    {
        Vector3 dir = target - gameObject.transform.position;
        float rotRadian = Mathf.Atan2(dir.z, dir.x);
        rotRadian += (rotRadian > 0f) ? -Mathf.PI : Mathf.PI;
        float rot = rotRadian * Mathf.Rad2Deg;
        //rot += (rot > 0f) ? -180f : 180f; // get opposite angle
        arrow.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, rot + 90f);

        Vector2 screenPos = Vector2.zero;
        float a = _UI.GetComponent<RectTransform>().rect.width;
        float b = _UI.GetComponent<RectTransform>().rect.height;
        a -= a / 8;
        b -= b / 8;

        Debug.Log("rot: "+rot+"   RotRadian:"+rotRadian);

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

        arrow.GetComponent<RectTransform>().anchoredPosition = screenPos;
    }
}
