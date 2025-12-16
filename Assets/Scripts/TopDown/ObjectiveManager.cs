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
        foreach (GameObject objective in Objectives)
        {
            ObjectiveObjects.Enqueue(objective);
        }
    }

    private void Update()
    {
        if (ObjectiveObjects.Peek().TryGetComponent(out ObjectiveBase ob))
        {
            if (ob.isCompleted)
            {
                ObjectiveObjects.Dequeue();
            }
        }
    }
}