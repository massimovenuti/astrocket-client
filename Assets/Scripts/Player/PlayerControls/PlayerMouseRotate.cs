using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseRotate : MonoBehaviour
{
    public Camera mainCamera;

    public float rotationSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 positionOnScreen = mainCamera.WorldToViewportPoint(transform.position);
        Vector3 mouseOnScreen = mainCamera.ScreenToViewportPoint(Input.mousePosition);
        float angle = Mathf.Atan2(positionOnScreen.y - mouseOnScreen.y, positionOnScreen.x - mouseOnScreen.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0f, -angle - 90f, 0f));

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}
