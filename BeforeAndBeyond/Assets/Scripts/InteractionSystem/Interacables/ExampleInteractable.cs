using UnityEngine;

public class ExampleInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt;
    
    public string InteractionPrompt => prompt;

    //only use if prompt is updated
    public bool PromptUpdated { get; set; } 
    
    public void  Interact(Interactor interactor)
    {
       Debug.Log("Event Triggered");
    }
}
