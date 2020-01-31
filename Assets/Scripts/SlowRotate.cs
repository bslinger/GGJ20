using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowRotate : MonoBehaviour
{
    bool rotating;
    float speed = 0;

    public void OnGUI()
    {
        if (!rotating)
        {
            GUILayout.Label("Press space to rotate");
        } else
        {
            GUILayout.Label("Rotating at " + speed);
            GUILayout.Label("Press space to stop rotation");
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            rotating = !rotating;
        }
        if (Input.GetKeyUp(KeyCode.KeypadPlus))
        {
            speed += 0.1f;
        }
        if (Input.GetKeyUp(KeyCode.KeypadMinus))
        {
            speed -= 0.1f;
        }

        if (rotating)
        {
            transform.Rotate(Vector3.forward * speed);
        }
    }
}
