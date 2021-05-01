using UnityEngine;

public class FirstScreenManager : ScreenManager
{
    public override void Start()
    {


        base.Start();
    }

    void Update()
    {
        if (Input.anyKeyDown)
            goToNextPage();
    }
}
