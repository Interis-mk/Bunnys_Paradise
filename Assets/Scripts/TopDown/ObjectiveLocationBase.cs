using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectiveBase : MonoBehaviour, IObjective
{
    public bool isActive{get;set;}
    
    public virtual bool isCompleted()
    {
        return true;
    }
}
