using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class WeaponHider : MonoBehaviour
{
    private EventBinding<CharacterSwap> characterSwapEventBinding; 
    [SerializeField] private CharacterType shellIntendedCharacter;
    private CharacterType currentCharacter; 


    private void OnEnable()
    {
        characterSwapEventBinding = new EventBinding<CharacterSwap>((e) =>SetWeaponsActive(e.CharacterType));
        EventBus<CharacterSwap>.Register(characterSwapEventBinding);
    }

    private void OnDisable()
    {
        EventBus<CharacterSwap>.Deregister(characterSwapEventBinding);
    }

    private void SetWeaponsActive(CharacterType type)
    {
        if (type != shellIntendedCharacter)
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.TryGetComponent<Canvas>(out Canvas canvas))
                {
                    canvas.enabled = false;
                    return;
                }
            
                child.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.TryGetComponent<Canvas>(out Canvas canvas))
                {
                    canvas.enabled = true;
                    return;
                }
            
                child.gameObject.SetActive(true);
            }
        }
    }
}
