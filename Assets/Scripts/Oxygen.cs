using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oxygen : SystemBase
{
    protected override void UpdateMe()
    {
        if (isPowered)
        {
            Increase();
        }
        else
        {
            Decrease();
        }
    }
}
