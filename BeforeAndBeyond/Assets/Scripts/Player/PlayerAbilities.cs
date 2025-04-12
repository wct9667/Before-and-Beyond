using System;
using System.Collections.Generic;
using System.Linq;
using Ability;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerState))]
    public class PlayerAbilities : MonoBehaviour
    {
        [Header("Input")] 
        [SerializeField] private InputReader inputReader;

        //cooldown tracker
        private Dictionary<AbstractAbility, float> abilityCooldowns = new();

        private PlayerState playerState;
        
        //events
        private EventBinding<CharacterSwap> characterSwapEventBinding;

        private void Awake()
        {
            playerState = GetComponent<PlayerState>();
            InitializeCoolDownsForCurrentCharacter();
        }

        private void Start()
        {
            EventBus<AbilitiesSwapped>.Raise(new AbilitiesSwapped()
            {
                cooldowns = abilityCooldowns
            });
        }

        private void OnEnable()
        {
            inputReader.StartingAbility += StartingAbility;
            characterSwapEventBinding = new EventBinding<CharacterSwap>(()=>
            {
                InitializeCoolDownsForCurrentCharacter();
                EventBus<AbilitiesSwapped>.Raise(new AbilitiesSwapped());
            });
            EventBus<CharacterSwap>.Register(characterSwapEventBinding);
        }
        
        private void OnDisable()
        {
            inputReader.StartingAbility -= StartingAbility;
            EventBus<CharacterSwap>.Deregister(characterSwapEventBinding);
        }

        private void Update()
        {
            //update cooldowns
            List<AbstractAbility> keys = abilityCooldowns.Keys.ToList();
            foreach (AbstractAbility ability in keys)
            {
                if (abilityCooldowns[ability] > 0f)
                    abilityCooldowns[ability] -= Time.deltaTime;
            }
        }

        /// <summary>
        /// Calls the starting ability from the data in player state
        /// Checks cooldown
        /// </summary>
        private void StartingAbility()
        {
            AbstractAbility ability = playerState.CurrentCharacter.StartingAbility;

            if (abilityCooldowns[ability] > 0f) return;
            ability.ActivateAbility();
            abilityCooldowns[ability] = ability.AbilityCooldown;
        }
        
        /// <summary>
        /// CoolDowns for a character, sets them up
        /// </summary>
        private void InitializeCoolDownsForCurrentCharacter()
        {
            abilityCooldowns.Clear();
            PlayerCharacterData character = playerState.CurrentCharacter;

            foreach (AbstractAbility ability in character.Abilities)
            {
                abilityCooldowns[ability] = 0f;
            }
        }
    }
}

