using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPowerCore : MonoBehaviour
{
    [SerializeField]
    GameObject toReset;

    private Vector3 initialPos;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = toReset.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == toReset)
        {
            toReset.transform.position = initialPos;
            toReset.GetComponent<Rigidbody>().velocity = Vector3.zero;
            toReset.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }
}
