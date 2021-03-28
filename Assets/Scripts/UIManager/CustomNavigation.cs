using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomNavigation : MonoBehaviour
{
    public void Update( )
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameObject c = EventSystem.current.currentSelectedGameObject;
            if (c == null) { return; }

            Selectable s = c.GetComponent<Selectable>();
            if (s == null) { return; }

            Selectable jump = Input.GetKey(KeyCode.LeftShift) ? s.FindSelectableOnUp() : s.FindSelectableOnDown();
            if (jump != null) { jump.Select(); }
        }
    }
}