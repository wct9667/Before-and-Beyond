using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Interactable_Rift : MonoBehaviour, IInteractable
{
    [Header("Enemy Settings")] 
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemyNum = 5;

    [Header("Prompt")] 
    [SerializeField] private string prompt;
    
    /// <summary>
    /// Spawnbounds for the enemies
    /// </summary>
    private BoxCollider spawnBounds;

    private bool promptUpdated;

    public string InteractionPrompt => prompt;

    private void Awake()
    {
        spawnBounds = GetComponent<BoxCollider>();
    }

    public bool PromptUpdated
    {
        get => promptUpdated;
        set => promptUpdated = value;
    }

    /// <summary>
    /// Spawns a bunch of enemies
    /// </summary>
    /// <param name="interactor">Object that interacted with the portal</param>
    public void Interact(Interactor interactor)
    {
        //spawn enemies
        for (int i = 0; i < enemyNum; i++)
        {
            
        }
    }
    
    

}
