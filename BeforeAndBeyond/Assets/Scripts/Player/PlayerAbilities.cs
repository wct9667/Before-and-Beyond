using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerState))]
    public class PlayerAbilities : MonoBehaviour
    {
        [Header("Input")] 
        [SerializeField] private InputReader inputReader;

        private PlayerState playerState;

        private void Awake()
        {
            playerState = GetComponent<PlayerState>();
        }

        private void OnEnable()
        {
            inputReader.StartingAbility += StartingAbility;
        }
        
        private void OnDisable()
        {
            inputReader.StartingAbility -= StartingAbility;
        }

        /// <summary>
        /// Calls the starting ability from the data in player state
        /// </summary>
        private void StartingAbility()
        {
            playerState.CurrentCharacter.StartingAbility.ActivateAbility();
        }
    }
}

