using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]

public class ClickerBase : MonoBehaviour
{
    [SerializeField]private float multiplier = 1;
    [SerializeField]private float giveAmount = 1;
    private CircleCollider2D collider;
    private Animator animator;
    
    public void Clicked()
    {
        animator.SetTrigger("clicked");
    }
}
