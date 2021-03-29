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

    private float _timer;
    private readonly float _alphaFactor = 15f;
    private readonly float _initialTimer = 10f;

    private bool _trigger = false;

    private void Awake( )
    {
        //_player = GameObject.FindGameObjectsWithTag("Player").First();

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

    private void Start( )
    {
        Reset();
    }

    private void Update( )
    {
        if (!isServer && !isLocalPlayer)
        {
            return;
        }

        if (_trigger)
        {
            if (isLocalPlayer)
            {
                _timerText.text = _timer.ToString("F1");
            }

            if (_timer <= 0 && _trigger)
            {
                if (isServer)
                {
                    Reset();
                    PlayerHealth ph = gameObject.GetComponent<PlayerHealth>();
                    ph.Revive();
                }
            }
            else
            {
                _timer = (_timer == 0) ? _timer : _timer - Time.deltaTime;
                if (isLocalPlayer)
                {
                    _tmpAlpha.a = _initialAlpha.a + (_initialTimer - _timer) / _alphaFactor;
                    _background.color = _tmpAlpha;
                }
            }
            if (isLocalPlayer)
            {
                CalcIndicator();
            }
        }
        else
        {
            if (_timer <= 10f)
            {
                _timer += Time.deltaTime;
            }
        }
    }

    [Client]
    private void OnEnable( )
    {
        if (isLocalPlayer)
        {
            _UI.SetActive(false);
            _background.color = _initialAlpha;
            Reset();
        }
    }

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
            RpcEnterMap();
        }
    }

    [ServerCallback]
    private void OnTriggerExit(Collider intruder)
    {
        if (intruder.CompareTag("WarningBorder"))
        {
            _trigger = true;
            RpcExitMap();
        }
    }

    [TargetRpc]
    private void RpcEnterMap()
    {
        if (isLocalPlayer)
        {
            _trigger = false;
            _UI.SetActive(false);
            _background.color = _initialAlpha;
        }
    }

    [TargetRpc]
    private void RpcExitMap( )
    {
        if (isLocalPlayer)
        {
            _trigger = true;
            _UI.SetActive(true);
        }
    }

    [Client]
    private void CalcIndicator( )
    {
        float rotRadian = Mathf.Atan2(gameObject.transform.position.z, gameObject.transform.position.x);
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
