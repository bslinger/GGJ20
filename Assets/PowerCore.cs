using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCore : MonoBehaviour
{

    private ConnectPoint hoverPoint;
    private ConnectPoint connectedPoint;

    public bool powered;
    public float initialAngularVelocity;
    public Material unpoweredMaterial;
    public Material poweredMaterial;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().angularVelocity = new Vector3(initialAngularVelocity, initialAngularVelocity);
        if (powered)
        {
            PowerUp();
        }
        else
        {
            PowerDown();
        }
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
        PowerUp();
    }

    public void OnAttachedToHand()
    {
        if (connectedPoint != null)
        {
            Debug.Log($"Attaching {name} to hand, disconnecting from {connectedPoint.name}");
            connectedPoint.DisconnectCore();
            connectedPoint = null;
        }
        PowerDown();
    }

    public void PowerDown()
    {
        powered = false;
        GameObject cylinder = GetComponentInChildren<MeshCollider>().gameObject;
        MeshRenderer meshRenderer = cylinder.GetComponent<MeshRenderer>();

        Material[] mats = meshRenderer.materials;
        mats[1] = unpoweredMaterial;
        meshRenderer.materials = mats;
    }

    public void PowerUp()
    {
        powered = true;
        GameObject cylinder = GetComponentInChildren<MeshCollider>().gameObject;
        MeshRenderer meshRenderer = cylinder.GetComponent<MeshRenderer>();
        Material[] mats = meshRenderer.materials;
        mats[1] = poweredMaterial;
        meshRenderer.materials = mats;
    }
}
