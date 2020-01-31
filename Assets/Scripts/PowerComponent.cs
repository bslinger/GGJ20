using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerComponent : MonoBehaviour, PowerSource
{
    bool isPowered;
    public void PowerUp (bool power)
    {
        isPowered = power;
    }

    public bool IsPowered()
    {
        return isPowered;
    }
}
