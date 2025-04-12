using Player;
using TMPro;
using UnityEngine;
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

    [Header("Sensitivity Settings")]
    [SerializeField] private Slider sensitivityXSlider;
    [SerializeField] private Slider sensitivityYSlider;
    [SerializeField] private TMP_InputField sensitivityXInput;
    [SerializeField] private TMP_InputField sensitivityYInput;
    [SerializeField] private PlayerController playerController;

    private EventBinding<Pause> pauseEventBinding;

    private void OnEnable()
    {
        pauseEventBinding = new EventBinding<Pause>((e) => { PauseUI(e.Paused); });
        EventBus<Pause>.Register(pauseEventBinding);

        sensitivityXSlider.onValueChanged.AddListener(OnSensitivityXSliderChanged);
        sensitivityYSlider.onValueChanged.AddListener(OnSensitivityYSliderChanged);
        sensitivityXInput.onEndEdit.AddListener(OnSensitivityXInputChanged);
        sensitivityYInput.onEndEdit.AddListener(OnSensitivityYInputChanged);
    }

    private void OnDisable()
    {
        EventBus<Pause>.Deregister(pauseEventBinding);

        sensitivityXSlider.onValueChanged.RemoveListener(OnSensitivityXSliderChanged);
        sensitivityYSlider.onValueChanged.RemoveListener(OnSensitivityYSliderChanged);
        sensitivityXInput.onEndEdit.RemoveListener(OnSensitivityXInputChanged);
        sensitivityYInput.onEndEdit.RemoveListener(OnSensitivityYInputChanged);
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

    private void Start()
    {
        // Init sliders as 0–1, sensX/sensY as 0–100
        sensitivityXSlider.value = SensitivityToNormalized(playerController.sensX);
        sensitivityYSlider.value = SensitivityToNormalized(playerController.sensY);

        sensitivityXInput.text = sensitivityXSlider.value.ToString("F1");
        sensitivityYInput.text = sensitivityYSlider.value.ToString("F1");
    }

    /// <summary>
    /// Pauses or unpauses the game based on passed in bool (from event
    /// </summary>
    private void PauseUI(bool paused)
    {
        pausedBackground.enabled = paused;

        if (!paused) return;

        mainPausedCanvas.enabled = true;
        settingsPausedCanvas.enabled = false;
        buttonResume.Select();
    }

    /// <summary>
    /// Sensitivity Handlers
    /// </summary>
    private float NormalizedToSensitivity(float normalized) => normalized * 100f;
    private float SensitivityToNormalized(float sensitivity) => sensitivity / 100f;
    private void OnSensitivityXSliderChanged(float value)
    {
        float actual = NormalizedToSensitivity(value);
        playerController.sensX = actual;
        sensitivityXInput.text = value.ToString("F1"); // Display 0–1
    }

    private void OnSensitivityYSliderChanged(float value)
    {
        float actual = NormalizedToSensitivity(value);
        playerController.sensY = actual;
        sensitivityYInput.text = value.ToString("F1");
    }

    private void OnSensitivityXInputChanged(string value)
    {
        if (float.TryParse(value, out float normalized))
        {
            normalized = Mathf.Clamp01(normalized);
            float actual = NormalizedToSensitivity(normalized);
            playerController.sensX = actual;
            sensitivityXSlider.value = normalized;
        }
    }

    private void OnSensitivityYInputChanged(string value)
    {
        if (float.TryParse(value, out float normalized))
        {
            normalized = Mathf.Clamp01(normalized);
            float actual = NormalizedToSensitivity(normalized);
            playerController.sensY = actual;
            sensitivityYSlider.value = normalized;
        }
    }
}