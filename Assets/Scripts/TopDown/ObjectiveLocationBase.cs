using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class ObjectiveBase : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnComplete;

    [Header("State")]
    public bool isCompleted;
    public bool isTouched;

    private bool hasFired;

    public enum ObjectiveType
    {
        Location,
        Item,
        Scene,
        Milestone
    }

    [Header("Objective Settings")]
    public ObjectiveType objectiveType;
    public float milestone;
    public string itemKey;
    public string sceneKey;

    private void Start()
    {
        // Safety: ensure event exists even if not assigned in Inspector
        if (OnComplete == null)
            OnComplete = new UnityEvent();
    }

    private void Update()
    {
        // Don't run completion logic in edit mode
        if (!Application.isPlaying)
            return;

        switch (objectiveType)
        {
            case ObjectiveType.Milestone:
                isCompleted = MilestoneComplete();
                break;

            case ObjectiveType.Scene:
                isCompleted = CompareSceneKey();
                break;

            case ObjectiveType.Item:
                // TODO: implement item logic later
                return;
        }

        if (isCompleted && !hasFired)
        {
            hasFired = true;
            OnComplete.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!Application.isPlaying)
            return;

        if (objectiveType == ObjectiveType.Location)
        {
            isCompleted = true;
            isTouched = true;

            if (!hasFired)
            {
                hasFired = true;
                OnComplete.Invoke();
            }
        }
    }

    public bool MilestoneComplete()
    {
        // Guard against missing CurrencyManager
        if (CurrencyManager.instance == null)
            return false;

        return CurrencyManager.instance.currency >= milestone;
    }

    public bool CompareSceneKey()
    {
        return SceneManager.GetActiveScene().name == sceneKey;
    }
}
