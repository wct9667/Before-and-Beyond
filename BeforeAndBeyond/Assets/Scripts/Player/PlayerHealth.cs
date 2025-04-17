using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100;
    private float currentHealth;
    
    //Event
    private EventBinding<ChangePlayerHealth> playerHealthChangeEventBinding;

    private void OnEnable()
    {
        playerHealthChangeEventBinding = new EventBinding<ChangePlayerHealth>((e) =>
        {
            ChangeHealth(e.healthChange);
            EventBus<ChangePlayerHealthUI>.Raise(new ChangePlayerHealthUI()
            {
                playerMaxHealth = maxHealth,
                playerHealth = currentHealth
            });
        });
        EventBus<ChangePlayerHealth>.Register(playerHealthChangeEventBinding);
    }

    private void OnDisable()
    {
        EventBus<ChangePlayerHealth>.Deregister(playerHealthChangeEventBinding);
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
    /// Change health
    /// </summary>
    /// <param name="healthChange"></param>
    private void ChangeHealth(float healthChange)
    {
        currentHealth += healthChange;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        if (currentHealth < 0) SceneLoader.LoadScene(0); //load the main menu
    }
}
