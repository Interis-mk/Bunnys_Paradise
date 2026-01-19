using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyWaitForSeconds : MonoBehaviour
{
    public static EasyWaitForSeconds instance;
    private bool isWaiting;

    private void Start()
    {
        instance = this;
    }

    public bool WaitSeconds(float seconds)
    {
        isWaiting = true;
        Coroutine c = StartCoroutine(nameof(WaitForSecondsRoutine), seconds);
        if(!isWaiting)
        {
            return true;
            StopCoroutine(c);
        }
        StopCoroutine(c);
        return false;
    }
    
    public IEnumerator WaitForSecondsRoutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        isWaiting = false;
        yield break;
    }
}
