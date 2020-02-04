using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetMusicPlayer : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            GameObject go = GameObject.FindGameObjectWithTag("music");
            Destroy(go);
        } catch { 
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
