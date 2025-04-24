using System;
using System.Collections.Generic;
using Ability;
using UnityEngine;

namespace Player
{
    public class PlayerState : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private InputReader inputReader;

        [Header("Characters")] 
        [SerializeField] private PlayerCharacterData knightCharacter;
        [SerializeField] private PlayerCharacterData hackerCharacter;
        
        [Header("Current/Starting Character")]
        [SerializeField] private PlayerCharacterData currentCharacter;

        public PlayerCharacterData CurrentCharacter => currentCharacter;
        public PlayerCharacterData[] Characters => new PlayerCharacterData[] {knightCharacter, hackerCharacter};

        private EventBinding<AddAbilitiesToCharacters> addAbilitiesToCharactersEventBinding;

        private void OnEnable()
        {
            inputReader.SwapCharacter += SwapCharacter;
            addAbilitiesToCharactersEventBinding = new EventBinding<AddAbilitiesToCharacters>(AddAbilitiesToCharacter);
            EventBus<AddAbilitiesToCharacters>.Register(addAbilitiesToCharactersEventBinding);
        }

        private void OnDisable()
        {
            inputReader.SwapCharacter -= SwapCharacter;
            EventBus<AddAbilitiesToCharacters>.Deregister(addAbilitiesToCharactersEventBinding);
        }

        private void Start()
        {
            //raise event for character change
            EventBus<CharacterSwap>.Raise(new CharacterSwap()
            {
                CharacterType =  currentCharacter.CharacterType
            });

            DataTracker.Instance.SetActiveCharacter(currentCharacter.CharacterType);
        }


        /// <summary>
        /// Changes the character to the opposite character
        /// </summary>
        private void SwapCharacter()
        {
            currentCharacter = currentCharacter.CharacterType == CharacterType.Knight
                ? hackerCharacter
                : knightCharacter;

            DataTracker.Instance.SetActiveCharacter(currentCharacter.CharacterType);
            DataTracker.Instance.PrintData();

            //raise event for character change
            EventBus<CharacterSwap>.Raise(new CharacterSwap()
            {
                CharacterType =  currentCharacter.CharacterType
            });
            
            Debug.Log($"Player Swapped to {currentCharacter.CharacterType}");
        }

        /// <summary>
        /// Adds an ability to a character
        /// </summary>
        /// <param name="ability">Ability to Add</param>
        /// <param name="characterType">Type to add it to</param>
        public void AddAbilitiesToCharacter(AddAbilitiesToCharacters eventData)
        {
            foreach (KeyValuePair<CharacterType, AbstractAbility> kvp in eventData.abilitiesToAdd)
            {
                if (kvp.Key == CharacterType.Hacker)
                {
                    if (hackerCharacter.Abilities.Contains(kvp.Value))
                    {
                        Debug.LogWarning("Character Already Has Ability" + kvp.Value);
                        return;
                    }
                    hackerCharacter.Abilities.Add(kvp.Value);
                }
                else
                {
                    if (knightCharacter.Abilities.Contains(kvp.Value))
                    {
                        Debug.LogWarning("Character Already Has Ability " + kvp.Value);
                        return;
                    }
                    knightCharacter.Abilities.Add(kvp.Value);
                }
            }

            //raise event for character change
            EventBus<CharacterSwap>.Raise(new CharacterSwap()
            {
                CharacterType =  currentCharacter.CharacterType
            });
            
        }
    }
}
