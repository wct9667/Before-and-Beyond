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
                if (child.gameObject.TryGetComponent<Canvas>(out Canvas canvas))
                {
                    canvas.enabled = false;
                    return;
                }
                child.gameObject.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        characterSwapEventBinding = new EventBinding<CharacterSwap>(SetWeaponsActive);
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
            if (child.gameObject.TryGetComponent<Canvas>(out Canvas canvas))
            {
                canvas.enabled = !canvas.enabled;
                return;
            }
            
            child.gameObject.SetActive(!child.gameObject.activeSelf);
        }
    }
}
