using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    private void Awake( )
    {
        GameObject go = GameObject.Find("CameraTarget");
        if (go == null)
            Debug.LogError("Couldn't find camera target");
        else
            this.transform.LookAt(go.transform);
    }
}
