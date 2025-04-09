using System;
using UnityEngine;


public class PauseController : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private InputReader inputReader;
        
        [Header("UI")]
        [SerializeField] private Canvas pausedBackground;
        [SerializeField] private Canvas mainPausedCanvas;
        [SerializeField] private Canvas settingsPausedCanvas;

        [Header("Buttons")] 
        [SerializeField] private UnityEngine.UI.Button buttonResume;
        [SerializeField] private UnityEngine.UI.Button buttonSettings;
        [SerializeField] private UnityEngine.UI.Button buttonSettingsBack;
        [SerializeField] private UnityEngine.UI.Button buttonMainMenu;

        [Header("Scene Index for Main Menu")]
        [SerializeField] private int mainMenuSceneIndex;

        private bool paused;
        
        private void OnEnable()
        {
            inputReader.Pause += PauseOrUnpause;
        }

        private void OnDisable()
        {
            inputReader.Pause -= PauseOrUnpause;
        }

        private void Start()
        {
            //set input map to game
            inputReader.SetGameInputMap();
        }

        private void Awake()
        {
            //setup OnClicks for buttons
            buttonMainMenu.onClick.AddListener(() =>
            {
                EventBus<ChangeScene>.Raise(new ChangeScene()
                {
                    sceneIndex = mainMenuSceneIndex
                });
            });
            
            buttonResume.onClick.AddListener(PauseOrUnpause);
            
            buttonSettings.onClick.AddListener(() =>
            {
                mainPausedCanvas.enabled = false;
                settingsPausedCanvas.enabled = true;
                buttonSettingsBack.Select();
            });
            
            buttonSettingsBack.onClick.AddListener(() =>
            {
                mainPausedCanvas.enabled = true;
                settingsPausedCanvas.enabled = false;
                buttonResume.Select();
            });
        }

        /// <summary>
        /// Pauses or unpauses the game based on internal pause bool.
        /// Sets input mode to UI, and enables paused canvas
        /// </summary>
        private void PauseOrUnpause()
        {
            paused = !paused;
            pausedBackground.enabled = paused;
            if (paused)
            {
                inputReader.SetUIInputMap();
                
                mainPausedCanvas.enabled = true;
                settingsPausedCanvas.enabled = false;
                
                Time.timeScale = 0;
                
                return;
            }
            
            inputReader.SetGameInputMap();

            Time.timeScale = 1;
        }
    }

