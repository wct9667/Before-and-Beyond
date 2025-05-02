using System.Collections.Generic;
using Enemy;
using Unity.VisualScripting;
using UnityEngine;

enum RiftState
{
    Active,
    Unstable
}
[RequireComponent(typeof(BoxCollider))]
public class Interactable_Rift : MonoBehaviour, IInteractable
{
    [Header("InteractionMasks - (Layer Numbers)")]
    [SerializeField] private int postInteractedMask;
    [SerializeField] private int interactionMask;

    [Header("Enemy Settings")] 
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemyNum = 5;

    [Header("Prompt")] 
    [SerializeField] private string prompt;
    [SerializeField] private string promptAfterEnemiesKilled;
    
    private List<GameObject> spawnedEnemies = new();
    [SerializeField] private RiftState riftState;
    
    /// <summary>
    /// Spawnbounds for the enemies
    /// </summary>
    private BoxCollider spawnBounds;

    private bool promptUpdated;

    public string InteractionPrompt => prompt;

    private void Awake()
    {
        spawnBounds = GetComponent<BoxCollider>();
    }

    public bool PromptUpdated
    {
        get => promptUpdated;
        set => promptUpdated = value;
    }

    /// <summary>
    /// Spawns a bunch of enemies
    /// </summary>
    /// <param name="interactor">Object that interacted with the portal</param>
    public void Interact(Interactor interactor)
    {
        switch (riftState)
        {
            case RiftState.Active:
                //spawn enemies
                for (int i = 0; i < enemyNum; i++)
                {
                    Vector3 spawnPos = GetRandomPointInBounds(spawnBounds.bounds);
                    GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                    spawnedEnemies.Add(enemy);
            
                    Health enemyHealth = enemy.GetComponent<Health>();
                    if (enemyHealth)
                    {
                        enemyHealth.DeathEvent += HandleEnemyDeath;
                    }
                }
                //set layer
                gameObject.layer = postInteractedMask;

                break;
            case RiftState.Unstable:
                EventBus<RiftClosed>.Raise(new RiftClosed());
                gameObject.SetActive(false);
                break;
        }
    }
    
    /// <summary>
    /// Gets a random point in a bounding box
    /// </summary>
    /// <param name="bounds"></param>
    /// <returns></returns>
    private Vector3 GetRandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
            UnityEngine.Random.Range(bounds.min.y, bounds.max.y),
            UnityEngine.Random.Range(bounds.min.z, bounds.max.z)
        );
    }
    
    /// <summary>
    /// Handles the enemy death
    /// </summary>
    /// <param name="enemy"></param>
    private void HandleEnemyDeath(Health enemy)
    {
        Debug.Log("DEATH");
        enemy.DeathEvent -= HandleEnemyDeath;
        spawnedEnemies.Remove(enemy.gameObject);

        if (spawnedEnemies.Count == 0)
        {
            Debug.Log("All enemies have been destroyed!");
            prompt = promptAfterEnemiesKilled;
            promptUpdated = false;
            riftState = RiftState.Unstable;
            gameObject.layer = interactionMask;
        }
    }
}
