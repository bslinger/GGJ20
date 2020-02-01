using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectPoint : MonoBehaviour, PowerSource
{

    public GameObject ghostCylinderPrefab;

    public PowerCore connectedCore;

    bool ghostShowing;
   

    // Start is called before the first frame update
    void Start()
    {
        if (connectedCore != null)
        {
            connectedCore.ConnectToPoint(this);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        PowerCore core = other.gameObject.GetComponentInParent<PowerCore>();
        if (core != null)
        {
            // show ghost mode
            Debug.Log("Ghost Mode On");
            ghostCylinderPrefab.SetActive(true);
           core.SetPointHovering(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PowerCore core = other.gameObject.GetComponentInParent<PowerCore>();
        if (core != null)
        {
            Debug.Log("Ghost Mode Off");
            ghostCylinderPrefab.SetActive(false);
            core.SetPointHovering(null);
        }
    }

    public void ConnectCore(PowerCore core)
    {
        connectedCore = core;
        connectedCore.transform.position = transform.position;
        connectedCore.transform.rotation = transform.rotation;
        connectedCore.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        GetComponent<CapsuleCollider>().enabled = false;
        ghostCylinderPrefab.SetActive(false);
    }

    public void DisconnectCore()
    {
        connectedCore.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetComponent<CapsuleCollider>().enabled = true;
        connectedCore = null;
    }

    public bool IsPowered()
    {
        if (connectedCore == null)
        {
            return false;
        }
        return connectedCore.powered;
    }
}

public interface PowerSource
{
    bool IsPowered();
}