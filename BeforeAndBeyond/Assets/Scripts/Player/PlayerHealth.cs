using Player;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Character ")]
    [SerializeField] private PlayerCharacterData playerCharacterData;

    [Header("Health")]
    [SerializeField] private float maxHealth = 100;
    private float currentHealth;
    
    //Event
    private EventBinding<IncreasePlayerHealth> playerIncreaseHealthEventBinding;
    private EventBinding<DecreasePlayerHealth> playerDecreaseHealthEventBinding;

    private void OnEnable()
    {
        playerIncreaseHealthEventBinding = new EventBinding<IncreasePlayerHealth>((e) =>
        {
            IncreaseHealth(e.healthChange);
            EventBus<ChangePlayerHealthUI>.Raise(new ChangePlayerHealthUI()
            {
                playerMaxHealth = maxHealth,
                playerHealth = currentHealth
            });
        });
        EventBus<IncreasePlayerHealth>.Register(playerIncreaseHealthEventBinding);

        playerDecreaseHealthEventBinding = new EventBinding<DecreasePlayerHealth>((e) =>
        {
            DecreaseHeatlh(e.healthChange);
            EventBus<ChangePlayerHealthUI>.Raise(new ChangePlayerHealthUI()
            {
                playerMaxHealth = maxHealth,
                playerHealth = currentHealth
            });
        });
        EventBus<DecreasePlayerHealth>.Register(playerDecreaseHealthEventBinding);
    }

    private void OnDisable()
    {
        EventBus<IncreasePlayerHealth>.Deregister(playerIncreaseHealthEventBinding);
        EventBus<DecreasePlayerHealth>.Deregister(playerDecreaseHealthEventBinding);
    }

    private void Start()
    {
        currentHealth = maxHealth;
        EventBus<ChangePlayerHealthUI>.Raise(new ChangePlayerHealthUI()
        {
            playerMaxHealth = maxHealth,
            playerHealth = currentHealth
        });
    }
    
    /// <summary>
    /// Increase health
    /// </summary>
    /// <param name="healthChange"></param>
    private void IncreaseHealth(float healthChange)
    {
        currentHealth += healthChange;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

    /// <summary>
    /// Decrease health
    /// </summary>
    /// <param name="healthChange"></param>
    private void DecreaseHeatlh(float healthChange)
    {
        currentHealth -= (healthChange * playerCharacterData.percentDamageReduction) / 100;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        if (currentHealth < 0) SceneLoader.LoadScene(0); //load the main menu
    }
}
