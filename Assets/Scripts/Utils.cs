﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class Utils
{
   public static IEnumerator LerpThen(GameObject gameObject, Vector3 destination, float time, Action thenDo)
    {
        float elapsedTime = 0f;
        Vector3 startingPos = gameObject.transform.position;
        while (elapsedTime < time)
        {
            gameObject.transform.position = Vector3.Lerp(startingPos, destination, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        thenDo();
    }
}
