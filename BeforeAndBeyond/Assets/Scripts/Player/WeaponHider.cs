using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHider : MonoBehaviour
{
    private EventBinding<CharacterSwap> characterSwapEventBinding;

    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.name == "SciFiWeaponShell")
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        characterSwapEventBinding = new EventBinding<CharacterSwap>(() =>
        {
            SetWeaponsActive();
            EventBus<AbilitiesSwapped>.Raise(new AbilitiesSwapped());
        });
        EventBus<CharacterSwap>.Register(characterSwapEventBinding);
    }

    private void OnDisable()
    {
        EventBus<CharacterSwap>.Deregister(characterSwapEventBinding);
    }

    private void SetWeaponsActive()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(!child.gameObject.activeSelf);
        }
    }
}
