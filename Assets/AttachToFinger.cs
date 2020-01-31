using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class AttachToFinger : MonoBehaviour
{
    GameObject hand;
    // Start is called before the first frame update
    void Start()
    {
        hand = GetComponentInParent<Hand>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnHandInitialized()
    {

        // find the finger
        Transform finger = hand.transform.Find("finger_index_0_r");
        if (finger != null)
        { this.transform.SetParent(finger); }
       
    }
}
