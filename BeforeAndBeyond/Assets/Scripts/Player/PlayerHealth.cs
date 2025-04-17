using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private Slider knightHealthBar;
    [SerializeField] private Slider sciFiHealthBar;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float currentHealth;
    [SerializeField] private Image knightHealthBarFill;
    [SerializeField] private Image sciFiHealthBarFill;
    
    
    private void Start()
    {
        currentHealth = maxHealth;
        knightHealthBar.maxValue = maxHealth;
        sciFiHealthBar.maxValue = maxHealth;
        knightHealthBar.value = currentHealth;
        sciFiHealthBar.value = currentHealth;
    }
    
    private void Update()
    {
        knightHealthBar.value = currentHealth;
        sciFiHealthBar.value = currentHealth;
        if (currentHealth <= 30)
        {
            knightHealthBarFill.color = Color.red;
            sciFiHealthBarFill.color = Color.red;
        }
    }
}
