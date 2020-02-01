using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using System.Linq;

public class StatusLights : MonoBehaviour
{
    
    [SerializeField] List<Color> statusColors;
    [SerializeField] List<Light> lights;
    [SerializeField] float transitionTime = 2.5f;

    private int currentStatus = -1;
    private Color targetColor;
    private float tColor = 0;

    private Color[] currentColors;
    
    // Start is called before the first frame update
    void Start()
    {
        lights = gameObject.GetComponentsInChildren<Light>().ToList();
        SetStatus(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (tColor <= 1 && currentColors.Length > 0 ){ // if end color not reached yet...
            tColor += Time.deltaTime / transitionTime; // advance timer at the right speed
            for (int i = 0; i < lights.Count; i++)
            {
                lights[i].color = Color.Lerp(currentColors[i], targetColor, tColor);
            }
        }
    }

    public void SetStatus(int newStatus)
    {
        if (newStatus == currentStatus)
        {
            return;
        }
        Debug.Log("SetStatus" + newStatus);
        currentColors = new Color[lights.Count];
        currentStatus = newStatus;
        for (int i = 0; i < lights.Count; i++)
        {
            currentColors[i] = lights[i].color;
        }
        targetColor = statusColors[newStatus];
        tColor = 0;
    }
}
