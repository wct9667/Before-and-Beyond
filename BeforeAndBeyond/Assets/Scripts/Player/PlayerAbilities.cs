using System;
using System.Collections.Generic;
using System.Linq;
using Ability;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    
    public enum AbilityType
    {
        Starting,
        Second,
        Third,
        Fourth
    }
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

        private Dictionary<AbilityType, UnityAction> abilityHandler;

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
            
            //Setup data for each characterdata
            foreach (PlayerCharacterData characterData in playerState.Characters)
            {
                foreach (AbstractAbility ability in characterData.Abilities)
                {
                    ability.Setup();
                }
            }
        }

        private void OnEnable()
        {
            abilityHandler = new Dictionary<AbilityType, UnityAction>
            {
                { AbilityType.Starting, () => PerformAbility(0) },
                { AbilityType.Second, () => PerformAbility(1) }
            };

            inputReader.StartingAbility += abilityHandler[AbilityType.Starting];
            
            inputReader.SecondAbility += abilityHandler[AbilityType.Second];
  
            characterSwapEventBinding = new EventBinding<CharacterSwap>(()=>
            {
                InitializeCoolDownsForCurrentCharacter();
                EventBus<AbilitiesSwapped>.Raise(new AbilitiesSwapped());
            });
            EventBus<CharacterSwap>.Register(characterSwapEventBinding);
        }
        
        private void OnDisable()
        {
            inputReader.StartingAbility -= abilityHandler[AbilityType.Starting];
            inputReader.SecondAbility -= abilityHandler[AbilityType.Second];
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
        private void PerformAbility(int index)
        {
            AbstractAbility ability = playerState.CurrentCharacter.AbilityAt(index);
            if (!ability) return;
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

            foreach (AbstractAbility ability in playerState.CurrentCharacter.Abilities)
            {
                abilityCooldowns[ability] = 0f;
            }
        }
    }
}

