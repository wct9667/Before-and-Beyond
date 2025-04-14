using Player;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PauseController : MonoBehaviour
{
    [Header("UI")] [SerializeField] private Canvas pausedBackground;
    [SerializeField] private Canvas mainPausedCanvas;
    [SerializeField] private Canvas settingsPausedCanvas;

    [Header("Buttons-MainPausedCanvas")] [SerializeField]
    private UnityEngine.UI.Button buttonResume;

    [SerializeField] private UnityEngine.UI.Button buttonSettings;
    [SerializeField] private UnityEngine.UI.Button buttonMainMenu;

    [Header("Buttons-SettingsPausedCanvas")] [SerializeField]
    private UnityEngine.UI.Button buttonSettingsBack;

    [Header("Scene Index for Main Menu")] [SerializeField]
    private int mainMenuSceneIndex;

    private EventBinding<Pause> pauseEventBinding;

    private void OnEnable()
    {
        pauseEventBinding = new EventBinding<Pause>((e) => { PauseUI(e.Paused); });
        EventBus<Pause>.Register(pauseEventBinding);
    }

    private void OnDisable()
    {
        EventBus<Pause>.Deregister(pauseEventBinding);
    }

    private void Awake()
    {
        //setup OnClicks for buttons
        buttonMainMenu.onClick.AddListener(() => { SceneLoader.LoadScene(mainMenuSceneIndex); });

        buttonResume.onClick.AddListener(() =>
        {
            EventBus<UnPause>.Raise(new UnPause());
            PauseUI(false);
        });

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
    /// Pauses or unpauses the game based on passed in bool (from event
    /// </summary>
    private void PauseUI(bool paused)
    {
        pausedBackground.enabled = paused;
        mainPausedCanvas.enabled = paused;

        if (!paused)
        {
            EventSystem.current.SetSelectedGameObject(null);
            return;
        }

        mainPausedCanvas.enabled = true;
        settingsPausedCanvas.enabled = false;
        buttonResume.Select();
    }
}