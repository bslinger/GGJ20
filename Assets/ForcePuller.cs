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

    private IEnumerator pullCoroutineRunning;

    // Start is called before the first frame update
    void Start()
    {
        activeAction.AddOnStateDownListener(this.PullTriggered, handRef);
        hand = GetComponentInParent<Hand>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 100, Color.yellow);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
           
            if (hit.collider.gameObject.GetComponentInParent<Interactable>() != null)
            {
                pointingAt = hit.collider.gameObject;
              // Debug.Log("Pointing at interactable", pointingAt);
            
            }
            else
            {
               
                if (pointingAt != null)
                {
                    Debug.Log("Clear pointing at");
                }
                pointingAt = null;
                
            }
        }
        else
        {
            if (pointingAt != null)
            {
                Debug.Log("Clear pointing at");
            }
            pointingAt = null;
        }
    }

    public void PullTriggered(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Trigger pulled", pointingAt);
        if (pointingAt != null)
        {
            if (pullCoroutineRunning != null)
            {
                StopCoroutine(pullCoroutineRunning);
            }
            GameObject toAttach = pointingAt.GetComponentInParent<Interactable>().gameObject;
            Debug.Log("to attach", toAttach);
            float timeToLerp = (pointingAt.transform.position = hand.transform.position).sqrMagnitude * .2f;
            StartCoroutine(Utils.LerpThen(toAttach, hand.transform.position, timeToLerp, () => {
                Debug.Log("Lerp ended, attaching", this.hand);
                this.hand.AttachObject(toAttach.GetComponentInParent<Interactable>().gameObject, GrabTypes.Grip); 
            }));
            
        }
    }
}
