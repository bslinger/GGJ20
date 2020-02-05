using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColor : MonoBehaviour
{

    [SerializeField] ParticleSystem particleSystem;
    [SerializeField] Color[] colors;

    // Start is called before the first frame update
    void Start()
    {
       var main = particleSystem.main;
       main.startColor = colors[Random.Range(0, colors.Length - 1)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
