using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowRotate : SystemBase
{
    [SerializeField] Transform target;

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
        target.Rotate(Vector3.forward * currentParamater);
    }
}
