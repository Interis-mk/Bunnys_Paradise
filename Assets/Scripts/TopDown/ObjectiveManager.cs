using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public List<GameObject> Objectives;
    public Queue<GameObject> ObjectiveObjects = new Queue<GameObject>();

    private void Start()
    {
        foreach (var o in Objectives)
        {
            o.SetActive(false);
        }

        foreach (GameObject objective in Objectives)
        {
            ObjectiveObjects.Enqueue(objective);
        }

        ObjectiveObjects.Peek().gameObject.SetActive(true);
    }

    private void Update()
    {
        if (ObjectiveObjects.Count >= 1)
        {
            if (ObjectiveObjects.Peek().TryGetComponent(out ObjectiveBase ob))
            {
                if (ob.isCompleted)
                {
                    DestroyImmediate(ob.gameObject);
                    ObjectiveObjects.Dequeue();
                    if (ObjectiveObjects.Count >= 1)
                    {
                        ObjectiveObjects.Peek().gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}