using System;
using System.Collections;
using System.Collections.Generic;
using Ability;
using Player;
using UnityEngine;

public class Interactable_ItemPickup : MonoBehaviour, IInteractable
{
    /// <summary>
    /// Small data class to hold abilities
    /// </summary>
    [System.Serializable]
    public class CharacterAbilityPair
    {
        public CharacterType characterType;
        public List<AbstractAbility> abilities;
    }

    [SerializeField] private List<CharacterAbilityPair> characterAbilities;
    
    [SerializeField] private string prompt;

    private Animator animator;
    public string InteractionPrompt => prompt;
    
    //flag for UI prompt change
    public bool PromptUpdated { get; set; }

    private void Start()
    {
        foreach (CharacterAbilityPair pair in characterAbilities)
        {
            foreach (var ability in pair.abilities)
            {
                ability.Setup();
            }
        }
        animator = GetComponent<Animator>();

    }

    public void Interact(Interactor interactor)
    {
        Dictionary<CharacterType, List<AbstractAbility>> abilitiesToAdd = new Dictionary<CharacterType, List<AbstractAbility>>();

        foreach (CharacterAbilityPair pair in characterAbilities)
        {
            if (!abilitiesToAdd.ContainsKey(pair.characterType))
                abilitiesToAdd[pair.characterType] = new List<AbstractAbility>();

            abilitiesToAdd[pair.characterType].AddRange(pair.abilities);
        }

        EventBus<AddAbilitiesToCharacters>.Raise(new AddAbilitiesToCharacters()
        {
            abilitiesToAdd = abilitiesToAdd
        });
        
        EventBus<IncreasePlayerHealth>.Raise(new IncreasePlayerHealth()
        {
            healthChange = 100
        });
        
        animator.SetTrigger("Open");
        gameObject.layer = LayerMask.GetMask("Default");
    }
}

