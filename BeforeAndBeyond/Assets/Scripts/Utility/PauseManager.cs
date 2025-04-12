using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputReader inputReader;

    private bool paused;

    private EventBinding<UnPause> unPauseEventBinding;

    private void OnEnable()
    {
        inputReader.Pause += PauseOrUnpause;
        unPauseEventBinding = new EventBinding<UnPause>(SwapPaused);
        EventBus<UnPause>.Register(unPauseEventBinding);
    }

    private void OnDisable()
    {
        inputReader.Pause -= PauseOrUnpause;
        EventBus<UnPause>.Deregister(unPauseEventBinding);
    }
    
    private void Start()
    {
        //set input map to game
        inputReader.SetGameInputMap();
    }


    /// <summary>
    /// Pauses or unpauses the game based on paused bool.
    /// Sets input map to UI and raises event so the UI knows
    /// </summary>
    private void PauseOrUnpause()
    {
        SwapPaused();
        
        EventBus<Pause>.Raise(new Pause()
        {
            Paused = paused
        });
    }

    private void SwapPaused()
    {
        paused = !paused;
        if (paused)
        {
            inputReader.SetUIInputMap();

            Time.timeScale = 0;

            Cursor.lockState = CursorLockMode.None;   // unlock cursor
            Cursor.visible = true;                    // show cursor
        }
        else
        {
            inputReader.SetGameInputMap();

            Time.timeScale = 1;

            Cursor.lockState = CursorLockMode.None;   // lock cursor
            Cursor.visible = false;                    // hide cursor
        }


    }
}
