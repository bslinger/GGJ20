using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCore : MonoBehaviour
{

    public ConnectPoint hoverPoint;
    public ConnectPoint connectedPoint;

    public bool powered;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPointHovering(ConnectPoint point)
    {
        hoverPoint = point;
    }

    public void ConnectToPoint()
    {
        if (hoverPoint == null)
        {
            Debug.LogError("Tried to connect to point when not hovering");
            return;
        }
        connectedPoint = hoverPoint;
        hoverPoint = null;
        
        connectedPoint.ConnectCore(this);

    }

    public void OnDetachedFromHand()
    {
        Debug.Log("Detached from hand");
        if (hoverPoint != null)
        {
            ConnectToPoint();
        }
    }

    public void OnAttachedToHand()
    {
        if (connectedPoint != null)
        {
            connectedPoint.DisconnectCore();
        }
    }
}
