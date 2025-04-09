using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputReader inputReader;


    private void OnEnable()
    {
        //setup input maps
        inputReader.Move += Move;
        inputReader.Look += Look;
        inputReader.Attack += Attack;
        inputReader.Interact += Interact;
        inputReader.Jump += Jump;
        inputReader.Sprint += Sprint;
        inputReader.StartingAbility += StartingAbility;
    }
    
    
    private void OnDisable()
    {
        //setup input maps
        inputReader.Move -= Move;
        inputReader.Look -= Look;
        inputReader.Attack -= Attack;
        inputReader.Interact -= Interact;
        inputReader.Jump -= Jump;
        inputReader.Sprint -= Sprint;
        inputReader.StartingAbility -= StartingAbility;
    }

    private void Move(Vector2 moveVector)
    {
        Debug.Log($"Move x:{moveVector.x}, y:{moveVector.y} ");
    }
    
    private void Look(Vector2 lookVector)
    {
        Debug.Log($"Look x:{lookVector.x}, y:{lookVector.y} ");
    }

    private void Attack()
    {
        Debug.Log("Attack");
    }
    
    private void Jump()
    {
        Debug.Log("Jump");
    }
    
    private void Interact()
    {
        Debug.Log("Interact");
    }
    
    private void Sprint(bool sprinting)
    {
        if(sprinting)
            Debug.Log("Sprint");
        else 
            Debug.Log("Stopped Sprint");
    }
    
    private void StartingAbility()
    {
        Debug.Log("StartingAbility");
    }
}
