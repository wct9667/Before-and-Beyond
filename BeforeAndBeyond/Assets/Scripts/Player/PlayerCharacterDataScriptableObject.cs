using System.Collections.Generic;
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
        public float jumpForce;
        public bool canDoubleJump;
        public bool canDash;

        [Header("Abilities")] 
        [SerializeField] private List<Ability.AbstractAbility> abilities;
        public List<Ability.AbstractAbility> Abilities => abilities;
        public Ability.AbstractAbility StartingAbility
        {
            get
            {
                if (abilities != null && abilities.Count > 0) return abilities[0];
                Debug.LogWarning("StartingAbility: abilities list is empty or null.");
                return null;
            }
        }

        public Ability.AbstractAbility SecondAbility
        {
            get
            {
                if (abilities != null && abilities.Count > 1) return abilities[1];
                Debug.LogWarning("SecondAbility: abilities list has fewer than 2 abilities.");
                return null;
            }
        }
    }
}