using System;
using System.Collections;
using System.Collections.Generic;
using Ability;
using Player;
using UnityEngine;

public class Interactable_ItemPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private AbstractAbility hackerAbility;
    [SerializeField] private AbstractAbility knightAbility;
    
    [SerializeField] private string prompt;
    public string InteractionPrompt => prompt;
    
    //flag for UI prompt change
    public bool PromptUpdated { get; set; }

    private void Start()
    {
        hackerAbility.Setup();
        knightAbility.Setup();
    }

    public void Interact(Interactor interactor)
    {
        EventBus<AddAbilitiesToCharacters>.Raise(new AddAbilitiesToCharacters()
        {
            abilitiesToAdd = new Dictionary<CharacterType, AbstractAbility>
            {
                {CharacterType.Hacker, hackerAbility},
                {CharacterType.Knight, knightAbility}
            }
        });
        
        EventBus<IncreasePlayerHealth>.Raise(new IncreasePlayerHealth()
        {
            healthChange = 100
        });
        
        
        gameObject.SetActive(false);
    }
}

