using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Luminosity.IO;

public class MockPlayerControls : MonoBehaviour
{
    private void Update( )
    {
        if(InputManager.GetButtonDown("Shoot"))
        {
            SaveAndLoadSettings.Save();
            Debug.Log("Boom");
        }

        if (InputManager.GetButtonDown("Boost"))
        {
            Debug.Log("Boosted");
        }
    }
}
