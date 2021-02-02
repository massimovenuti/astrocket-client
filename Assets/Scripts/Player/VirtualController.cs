using UnityEngine;

public class VirtualController : MonoBehaviour
{
    private static VirtualController _instance;
    private static VirtualController VirtualControllerInstance { get { return _instance; } }

    private KeyCode _left;
    private KeyCode _right;
    private KeyCode _up;
    private KeyCode _down;
    private KeyCode _boost;
    private KeyCode _shoot;


    private void Awake( )
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;

        }
    }

    public float VirtualHorizontalAxis()
    {
        if(Input.GetKey(_left) || Input.GetKey(_right))
            return Input.GetAxis("Horizontal");
        return 0f;
    }

    public float VirtualVerticalAxis()
    {
        if (Input.GetKey(_up) || Input.GetKey(_down))
            return Input.GetAxis("Vertical");
        return 0f;
    }

    public bool IsShooting()
    {
        if(Input.GetKey(_shoot))
            return true;
        return false;
    }

    public bool IsBoosting( )
    {
        if (Input.GetKey(_boost))
            return true;
        return false;
    }
}
