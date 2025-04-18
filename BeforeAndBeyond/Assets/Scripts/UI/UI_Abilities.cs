using System;
using System.Collections.Generic;
using Ability;
using Player;
using UnityEngine;

public class UI_Abilities : MonoBehaviour
{
    [SerializeField] private UI_Ability abilityPrefab;
    
    private List<UI_Ability> icons = new List<UI_Ability>();
    
    private Dictionary<AbstractAbility, AbilityData> coolDowns; //reference to cooldowns


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
        //remove old ones
        if (icons.Count != 0)
        {
            for (int i = icons.Count - 1; i >= 0; i--)
            {
                Destroy(icons[i].gameObject);
            }
        }
        
        icons.Clear();
        
        foreach (KeyValuePair<AbstractAbility, AbilityData> kvp in coolDowns)
        {
            UI_Ability ability = Instantiate(abilityPrefab, transform);
            icons.Add(ability);
            ability.Icon.sprite = kvp.Key.Image;
            ability.Fill.fillAmount = 1;
        }
    }

    private void UpdateAbilities()
    {
        int index = coolDowns.Count -1;
        foreach (KeyValuePair<AbstractAbility, AbilityData> kvp in coolDowns)
        {
            if (!kvp.Value.IsActive) continue;
           icons[index].Fill.fillAmount = (kvp.Value.Cooldown)/ kvp.Key.AbilityCooldown;
           index--;
        }
    }

    private void Update()
    {
        if (coolDowns == null) return;
        UpdateAbilities();
    }
}
