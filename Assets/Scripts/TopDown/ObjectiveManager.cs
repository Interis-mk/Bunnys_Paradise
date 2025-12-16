using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public List<GameObject> ObjectiveObjects = new List<GameObject>();

    private void Update()
    {
        foreach (GameObject objective in ObjectiveObjects)
        {
            if (objective.TryGetComponent(out ObjectiveBase ob))
            {
                Debug.Log(ob.objectiveType);
            }
        }
    }
}
