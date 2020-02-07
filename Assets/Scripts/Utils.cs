using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class Utils
{
   public static IEnumerator LerpThen(GameObject gameObject, Vector3 destination, float time, Action thenDo)
    {
        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.isKinematic = true;
        }
        
        float elapsedTime = 0f;
        Vector3 startingPos = gameObject.transform.position;
        Quaternion startingRot = gameObject.transform.rotation;
        while (elapsedTime < time)
        {
            gameObject.transform.position = Vector3.Lerp(startingPos, destination, (elapsedTime / time));
            gameObject.transform.rotation = Quaternion.Lerp(startingRot, Quaternion.identity, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        if (rigidbody != null)
        {
            rigidbody.isKinematic = false;
        }
        thenDo();
    }
}
