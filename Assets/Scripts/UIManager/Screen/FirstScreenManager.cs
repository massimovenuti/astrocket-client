using UnityEngine;

public class FirstScreenManager : ScreenManager
{
    new void Start()
    {


        base.Start();
    }

    void Update()
    {
        if (Input.anyKeyDown)
            goToNextPage();
    }
}
