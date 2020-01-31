using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectPoint : MonoBehaviour
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
        if (other.gameObject.CompareTag("PowerCore"))
        {
            // show ghost mode
            Debug.Log("Ghost Mode On");
            ghostCylinderPrefab.SetActive(true);
            other.gameObject.GetComponent<PowerCore>().SetPointHovering(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PowerCore"))
        {
            Debug.Log("Ghost Mode Off");
            ghostCylinderPrefab.SetActive(false);
            other.gameObject.GetComponent<PowerCore>().SetPointHovering(null);
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
