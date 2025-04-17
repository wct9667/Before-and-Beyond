using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Health : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image fillHealthBar;
    [SerializeField] private Slider sliderHealthBar;

    private EventBinding<ChangePlayerHealthUI> changePlayerHealthUIEventBinding;


    private void Awake()
    {
        sliderHealthBar = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        changePlayerHealthUIEventBinding = new EventBinding<ChangePlayerHealthUI>((e) =>
        {
            UpdateUI(e.playerMaxHealth, e.playerHealth);
        });
        EventBus<ChangePlayerHealthUI>.Register(changePlayerHealthUIEventBinding);
    }

    private void OnDisable()
    {
        EventBus<ChangePlayerHealthUI>.Deregister(changePlayerHealthUIEventBinding);
    }

    private void UpdateUI(float maxHealth, float currentHealth)
    {
        sliderHealthBar.maxValue = maxHealth;
        sliderHealthBar.value = currentHealth;
        if (currentHealth <= 30)
        {
            fillHealthBar.color = Color.red;
            fillHealthBar.color = Color.red;
        }
    }
}

