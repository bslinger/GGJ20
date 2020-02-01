using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ForcePuller : MonoBehaviour
{

    public float pullForce = 100f;

    public SteamVR_Action_Boolean activeAction;

    public SteamVR_Input_Sources handRef;

    public float slowDistance = 3f;

    public float snapDistance = 2f;

    private GameObject pointingAt;
    private Hand hand;

    // Start is called before the first frame update
    void Start()
    {
        //activeAction.AddOnStateDownListener(this.PullTriggered, handRef);
        hand = GetComponentInParent<Hand>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 100, Color.yellow);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
           
            if (hit.collider.gameObject.GetComponent<Interactable>() != null)
            {
                pointingAt = hit.collider.gameObject;
                Debug.Log("hit interactable");
                pointingAt = hit.collider.gameObject;
                if (activeAction.state)
                {
                    Vector3 difference = (transform.position - pointingAt.transform.position);
                    float sqrMagnitude = difference.sqrMagnitude;
                    Debug.Log($"Distance {sqrMagnitude}");
                    float finalPullForce = pullForce;
                    if (sqrMagnitude < slowDistance)
                    {
                        //finalPullForce -= pullForce / (slowDistance - sqrMagnitude);
                        Debug.Log($"Final pull force: {finalPullForce}");
                    }
                    if (sqrMagnitude < snapDistance)
                    {
                        hand.AttachObject(pointingAt, GrabTypes.Grip);
                    }
                    else
                    {
                        pointingAt.GetComponent<Rigidbody>().AddForce(difference.normalized * finalPullForce);
                    }
                    

                }
                
            }
            else
            {
                pointingAt = null;
            }
        }
        else
        {
            pointingAt = null;
        }
    }

    public void PullTriggered(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Trigger pulled");
        if (pointingAt != null)
        {
            
        }
        
    }
}
