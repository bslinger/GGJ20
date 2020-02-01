using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCore : MonoBehaviour
{

    private ConnectPoint hoverPoint;
    private ConnectPoint connectedPoint;

    public bool powered;
    public float initialAngularVelocity;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().angularVelocity = new Vector3(initialAngularVelocity, initialAngularVelocity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPointHovering(ConnectPoint point)
    {
        hoverPoint = point;
    }

    public void ConnectToPoint(ConnectPoint point)
    {

        connectedPoint = point;
        hoverPoint = null;
        
        connectedPoint.ConnectCore(this);

    }

    public void OnDetachedFromHand()
    {
        Debug.Log("Detached from hand");
        if (hoverPoint != null)
        {
            ConnectToPoint(hoverPoint);
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
