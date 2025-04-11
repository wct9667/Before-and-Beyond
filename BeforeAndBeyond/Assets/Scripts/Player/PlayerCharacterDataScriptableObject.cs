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
        public bool canDoubleJump;
        public bool canDash;

        [Header("Abilities")] 
        [SerializeField] private Ability.AbstractAbility startingAbility;
        public Ability.AbstractAbility StartingAbility => startingAbility;
        
        //[SerializeField] private Ability.AbstractAbility firstAbility;
        //[SerializeField] private Ability.AbstractAbility secondAbility;
        //[SerializeField] private Ability.AbstractAbility thirdAbility;
    }
}