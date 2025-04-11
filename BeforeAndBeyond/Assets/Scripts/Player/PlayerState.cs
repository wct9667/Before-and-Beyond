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

        private void OnEnable()
        {
            inputReader.SwapCharacter += SwapCharacter;
        }

        private void OnDisable()
        {
            inputReader.SwapCharacter -= SwapCharacter;
        }


        /// <summary>
        /// Changes the character to the opposite character
        /// </summary>
        private void SwapCharacter()
        {
            currentCharacter = currentCharacter.CharacterType == CharacterType.Knight
                ? hackerCharacter
                : knightCharacter;

            //raise event for character change
            EventBus<CharacterSwap>.Raise(new CharacterSwap()
            {
                CharacterType =  currentCharacter.CharacterType
            });
        }
    }
}
