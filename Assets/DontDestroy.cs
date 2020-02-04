using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{

    [SerializeField] string tag;
    [SerializeField] bool dontDestroy = true;
    // Start is called before the first frame update
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        if(dontDestroy) DontDestroyOnLoad(this.gameObject);
    }

}
