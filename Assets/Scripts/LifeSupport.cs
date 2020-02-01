using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSupport : SystemBase
{
    public bool oxygenOn;
    public bool carbonDioxideOn;

    [SerializeField] PowerComponent oxygenOutlet;
    [SerializeField] PowerComponent carbonDioxideOutlet;

    // Start is called before the first frame update
    protected override void StartMe()
    {
        
    }

    // Update is called once per frame
    protected override void UpdateMe()
    {
        if (isPowered)
        {
            Increase();
        } else
        {
            Decrease();
        }

        oxygenOutlet.PowerUp(false);
        carbonDioxideOutlet.PowerUp(false);
        if (isPowered)
        {
            if (oxygenOn)
            {
                Decrease(0.75f);
                oxygenOutlet.PowerUp(true);
            }


            if (carbonDioxideOn)
            {
                Decrease(0.5f);
                carbonDioxideOutlet.PowerUp(true);
            }

        }
    }
}

