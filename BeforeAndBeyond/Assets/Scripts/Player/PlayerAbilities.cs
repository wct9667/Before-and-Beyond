using System;
using System.Collections.Generic;
using System.Linq;
using Ability;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    
    [System.Serializable]
    public class AbilityData
    {
        public float Cooldown;
        public bool IsActive;

        public AbilityData(float cooldown, bool isActive)
        {
            Cooldown = cooldown;
            IsActive = isActive;
        }
    }

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
        private Dictionary<AbstractAbility, AbilityData> abilityCooldowns = new();

        private PlayerState playerState;
        
        //events
        private EventBinding<CharacterSwap> characterSwapEventBinding;

        private Dictionary<AbilityType, UnityAction> abilityHandler;

        private void Awake()
        {
            playerState = GetComponent<PlayerState>();
            
            //Setup data for each ability
            foreach (PlayerCharacterData characterData in playerState.Characters)
            {
                foreach (AbstractAbility ability in characterData.Abilities)
                {
                    abilityCooldowns[ability] = new AbilityData(0, false);
                }
            }
            
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
                { AbilityType.Second, () => PerformAbility(1) },
                { AbilityType.Third, () => PerformAbility(2) },
                { AbilityType.Fourth, () => PerformAbility(3) }

            };

            inputReader.StartingAbility += abilityHandler[AbilityType.Starting];
            
            inputReader.SecondAbility += abilityHandler[AbilityType.Second];

            inputReader.ThirdAbility += abilityHandler[AbilityType.Third];

            inputReader.FourthAbility += abilityHandler[AbilityType.Fourth];

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
            inputReader.ThirdAbility -= abilityHandler[AbilityType.Third];
            inputReader.FourthAbility -= abilityHandler[AbilityType.Fourth];

            EventBus<CharacterSwap>.Deregister(characterSwapEventBinding);
        }

        private void Update()
        {
            //update cooldowns
            List<AbstractAbility> keys = abilityCooldowns.Keys.ToList();
            foreach (AbstractAbility ability in keys)
            {
                if (abilityCooldowns[ability].Cooldown > 0f)
                    abilityCooldowns[ability].Cooldown -= Time.deltaTime;

                if (ability.UsesCharges)
                {
                    ability.TickRecharge(Time.deltaTime);
                    
                    if(abilityCooldowns[ability].Cooldown <= 0 && ability.Charges < ability.MaxCharges)
                    {
                        abilityCooldowns[ability].Cooldown = ability.AbilityCooldown;
                    }
                }
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
            
            //if the ability uses charges and doesnt have enough don't perform
            if (!ability.UpdateAndCheckCharges()) return;
            
            //if the ability doesn't use charges, return if its cooldown is bad
            if (!ability.UsesCharges && abilityCooldowns[ability].Cooldown > 0f) return;
            
            ability.ActivateAbility();

            DataTracker.Instance.RegisterAbilityUse(playerState.CurrentCharacter.CharacterType, index);

            //do not update cooldown if charge 
            if (ability.UsesCharges) return;
            
            abilityCooldowns[ability].Cooldown = ability.AbilityCooldown;
        }

        /// <summary>
        /// CoolDowns for a character, sets them up
        /// </summary>
        private void InitializeCoolDownsForCurrentCharacter()
        {
            //loop through dictionary
            foreach (KeyValuePair<AbstractAbility, AbilityData> kvp in abilityCooldowns)
            {
                //its in the character
                if (playerState.CurrentCharacter.Abilities.Contains(kvp.Key)) kvp.Value.IsActive = true;
                else kvp.Value.IsActive = false;
            }
            CheckForNewAbilities(true);

        }

        private void CheckForNewAbilities(bool active = false)
        {
            foreach (AbstractAbility ability in playerState.CurrentCharacter.Abilities)
            {
                if (!abilityCooldowns.ContainsKey(ability)) 
                    abilityCooldowns[ability] = new AbilityData(0, active);
            }
        }
    }
}

