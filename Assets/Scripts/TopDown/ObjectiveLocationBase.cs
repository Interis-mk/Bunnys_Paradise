using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[ExecuteInEditMode]
public class ObjectiveBase : MonoBehaviour, IObjective
{
    public bool isActive{get;set;}
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
    
    
    public bool isCompleted()
    {
        return true;
    }
}

