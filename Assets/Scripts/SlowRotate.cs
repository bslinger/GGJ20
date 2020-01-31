using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowRotate : SystemBase
{
    // Update is called once per frame
    protected override void UpdateMe()
    {
        if (isPowered)
        {
            Decrease();
        }
        else
        {
            Increase();
        }
        transform.Rotate(Vector3.forward * currentParamater);
    }
}
