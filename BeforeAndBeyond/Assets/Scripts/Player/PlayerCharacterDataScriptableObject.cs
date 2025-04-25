using System;
using System.Collections.Generic;
using Ability;
using UnityEngine;

namespace Player
{
    public enum CharacterType
    {
        Knight,
        Hacker
    }
    
    [CreateAssetMenu(menuName = "Player/CharacterData", fileName = "CharacterDataSO")]
    public class PlayerCharacterData : ScriptableObject
    {
        [Header("Type")] 
        [SerializeField] private CharacterType characterType;

        public CharacterType CharacterType => characterType;
        
        [Header("Passive Modifiers")]
        [Range(0,100)]  public float percentDamageReduction;
        [Range(0,1000)] public float percentSpeedIncrease;
        [Range(0, 10)] public float airMovementScalar; 
        public float jumpForce;
        public bool canDoubleJump;
        public bool canDash;

        [Header("Abilities")] 
        [SerializeField] private List<Ability.AbstractAbility> startingAbilities;
        [SerializeField] private List<Ability.AbstractAbility> abilities;
        public List<Ability.AbstractAbility> Abilities => abilities;
        public Ability.AbstractAbility AbilityAt(int index)
        {
            if (abilities != null && abilities.Count > index) return abilities[index];
            Debug.LogWarning("Requested ability does not exist, abilities list count exceeded.");
            return null;
        }

        private void OnEnable()
        {
            abilities.Clear();
            abilities = new List<AbstractAbility>(startingAbilities);
        }
    }
}