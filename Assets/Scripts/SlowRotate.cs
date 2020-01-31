using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowRotate : MonoBehaviour
{
    bool rotating;
    float speed = 0;
    [SerializeField] float maxSpeed = 0.1f;
    [SerializeField] float step = 0.01f;
    [SerializeField] bool isDebug;

    public void OnGUI()
    {
        if (isDebug)
        {
            if (!rotating)
            {
                GUILayout.Label("Press space to rotate");
            }
            else
            {
                GUILayout.Label("Rotating at " + speed);
                GUILayout.Label("Press space to stop rotation");
            }
        }
    }

    public float IncreseSpeed()
    {
        speed += step;
        speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);
        return speed;
    }

    public float DecreaseSpeed()
    {
        speed -= step;
        speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);
        return speed;
    }


    // Update is called once per frame
    void Update()
    {
        if (isDebug)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                rotating = !rotating;
            }
            if (Input.GetKeyUp(KeyCode.KeypadPlus))
            {
                speed += step;
            }
            if (Input.GetKeyUp(KeyCode.KeypadMinus))
            {
                speed -= step;
            }
            speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);
        }

        if (rotating)
        {
            transform.Rotate(Vector3.forward * speed);
        }
    }
}
