using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthJourney : MonoBehaviour
{
    [SerializeField] Comms comms;
    [SerializeField] Vector3 endPosition;
    Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;    
    }

    // Update is called once per frame
    void Update()
    {
        float percentage = comms.PercentageOfJourney;

        transform.position = Vector3.Lerp(startPosition, endPosition, percentage);

    }
}
