using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class StatusLights : MonoBehaviour
{
    
    [SerializeField] Color[] statusColours;
    [SerializeField] Light[] lights;
    
    // Start is called before the first frame update
    void Start()
    {
        lights = gameObject.GetComponentsInChildren<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetStatus(int newStatus)
    {
        foreach (Light light in lights)
        {
            light.GetComponent<Light>().color = statusColours[newStatus];
        }
    }
}
