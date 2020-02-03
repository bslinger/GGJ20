using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : SystemBase
{

    override protected void UpdateMe()
    {
        if (isPowered)
        {
            Decrease();
        }
        else
        {
            Increase();
        }
    }
}
