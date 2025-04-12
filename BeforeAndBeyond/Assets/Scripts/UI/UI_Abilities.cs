using System;
using System.Collections.Generic;
using Ability;
using UnityEngine;

public class UI_Abilities : MonoBehaviour
{
    [SerializeField] private UI_Ability abilityPrefab;
    
    private List<UI_Ability> icons = new List<UI_Ability>();
    
    private Dictionary<AbstractAbility, float> coolDowns; //reference to cooldowns


    private EventBinding<AbilitiesSwapped> abilitiesSwappedEventBinding;

    private void OnEnable()
    {
        abilitiesSwappedEventBinding = new EventBinding<AbilitiesSwapped>((e) =>
        {
            if (e.cooldowns != null)
                coolDowns = e.cooldowns;
            
            ResetAbilities();
        });
        EventBus<AbilitiesSwapped>.Register(abilitiesSwappedEventBinding);
    }

    private void OnDisable()
    {
        EventBus<AbilitiesSwapped>.Deregister(abilitiesSwappedEventBinding);
    }

    private void ResetAbilities()
    {
        if (icons.Count != 0)
        {
            for (int i = icons.Count - 1; i >= 0; i--)
            {
                Debug.Log("Destroy");
                Destroy(icons[i].gameObject);
            }
        }
        
        icons.Clear();

        int index = 0;
        foreach (KeyValuePair<AbstractAbility, float> kvp in coolDowns)
        {
            icons.Add(Instantiate(abilityPrefab, transform));
            icons[index].Icon.sprite = kvp.Key.Image;
            icons[index].Fill.fillAmount = 1;
        }

    }

    private void UpdateAbilities()
    {
        int index = 0;
        foreach (KeyValuePair<AbstractAbility, float> kvp in coolDowns)
        {
           icons[index].Fill.fillAmount = (kvp.Value)/ kvp.Key.AbilityCooldown;
        }
    }

    private void Update()
    {
        if (coolDowns == null) return;
        UpdateAbilities();
    }
}
