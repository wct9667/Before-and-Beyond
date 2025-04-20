
public interface IInteractable
{
    public string InteractionPrompt { get; }
    
    //flag for UI prompt change
    bool PromptUpdated { get; set; } 

    public void Interact(Interactor interactor);
}
