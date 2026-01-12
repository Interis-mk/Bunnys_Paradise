using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class ObjectiveBase : MonoBehaviour
{
    public bool isCompleted;
    public enum ObjectiveType
    {
        Location,
        Item,
        Scene,
        Milestone
    }
    public ObjectiveType objectiveType;
    public float milestone;
    public string itemKey;
    public bool isTouched;
    public string sceneKey;

    public bool MilestoneComplete()
    { 
        return CurrencyManager.instance.currency >= milestone;
    }

    public bool CompareSceneKey()
    {
        return SceneManager.GetActiveScene().name == sceneKey;
    }

    private void Update()
    {
        if(objectiveType == ObjectiveType.Milestone)
        {
            isCompleted = MilestoneComplete();
        }

        if (objectiveType == ObjectiveType.Scene)
        {
            CompareSceneKey();
        }

        if (objectiveType == ObjectiveType.Item)
        {
            throw new NotImplementedException();
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (objectiveType == ObjectiveType.Location)
        {
            isCompleted = true;
            isTouched = true;
        }
    }
}

