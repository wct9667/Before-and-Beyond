using Player;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private PlayerState playerState;
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
        playerState = GetComponent<PlayerState>();
        currentHealth = maxHealth;
        EventBus<ChangePlayerHealthUI>.Raise(new ChangePlayerHealthUI()
        {
            playerMaxHealth = maxHealth,
            playerHealth = currentHealth
        });
    }
    
    /// <summary>
    /// Increase health of the player, if they reach max cap it.
    /// </summary>
    /// <param name="healthChange">Health to increase player</param>
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
        currentHealth -= healthChange * (1 - playerState.CurrentCharacter.percentDamageReduction / 100f);
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        if (currentHealth < 0) GameOver(); //load the main menu
    }
    
    private void GameOver()
    {
        SceneLoader.LoadScene(0);
        DataTracker.Instance.SaveToFile();
    }
}
