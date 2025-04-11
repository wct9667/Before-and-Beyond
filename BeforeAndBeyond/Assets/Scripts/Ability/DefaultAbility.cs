
using System;
using UnityEngine;

namespace Ability
{
    [Serializable]
    public class DefaultAbility : IAbility
    {

        [SerializeField] private string name;

        public string Name => name; 
        
        
        public void ActivateAbility()
        {
            Debug.Log($"Activating Ability {name}");
        }
    }
}

