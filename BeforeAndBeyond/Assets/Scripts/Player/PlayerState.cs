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
        /// <param name="eventData"> Takes a struct that contains a dictionary of characterTypes to a list of abilities. </param>
        private void AddAbilitiesToCharacter(AddAbilitiesToCharacters eventData)
        {
            foreach (KeyValuePair<CharacterType, List<AbstractAbility>> kvp in eventData.abilitiesToAdd)
            {
                PlayerCharacterData character = GetCharacterByType(kvp.Key);
                if (character == null)
                {
                    Debug.LogWarning($"Character of type {kvp.Key} not found.");
                    continue;
                }

                foreach (AbstractAbility ability in kvp.Value)
                {
                    if (character.Abilities.Contains(ability))
                    {
                        Debug.LogWarning($"Character {kvp.Key} already has ability {ability.name}");
                        continue;
                    }

                    character.Abilities.Add(ability);
                }
            }

            //raise event for character change
            EventBus<CharacterSwap>.Raise(new CharacterSwap()
            {
                CharacterType = currentCharacter.CharacterType
            });
        }
        
        /// <summary>
        /// Retunrs 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private PlayerCharacterData GetCharacterByType(CharacterType type)
        {
            return type switch
            {
                CharacterType.Hacker => hackerCharacter,
                CharacterType.Knight => knightCharacter,
                _ => null
            };
        }
    }
}
