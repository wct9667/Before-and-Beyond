using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Settings: MonoBehaviour
{
    [Header("Sensitivity Settings")]
    [SerializeField] private Slider sensitivityXSlider;
    [SerializeField] private Slider sensitivityYSlider;
    [SerializeField] private TMP_InputField sensitivityXInput;
    [SerializeField] private TMP_InputField sensitivityYInput;

    [Header("Additional Settings")] 
    [SerializeField] private Toggle helmetToggle;

    [Header("Reset")] 
    [SerializeField] private Button resetButton;
    


    [SerializeField] private Settings settings;
    [SerializeField] private Settings defaultSettings;

    private void OnEnable()
    {
        sensitivityXSlider.onValueChanged.AddListener(OnSensitivityXSliderChanged);
        sensitivityYSlider.onValueChanged.AddListener(OnSensitivityYSliderChanged);
        sensitivityXInput.onEndEdit.AddListener(OnSensitivityXInputChanged);
        sensitivityYInput.onEndEdit.AddListener(OnSensitivityYInputChanged);
        helmetToggle.onValueChanged.AddListener(OnHelmetToggleChange);
        resetButton.onClick.AddListener(() =>
        {
            settings.SetValues(defaultSettings);
            SetValues();
        });
    }

    private void OnDisable()
    {
        sensitivityXSlider.onValueChanged.RemoveListener(OnSensitivityXSliderChanged);
        sensitivityYSlider.onValueChanged.RemoveListener(OnSensitivityYSliderChanged);
        sensitivityXInput.onEndEdit.RemoveListener(OnSensitivityXInputChanged);
        sensitivityYInput.onEndEdit.RemoveListener(OnSensitivityYInputChanged);
        helmetToggle.onValueChanged.RemoveListener(OnHelmetToggleChange);
        resetButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
       SetValues();
    }

    private void SetValues()
    {
        // Init sliders as 0�1, sensX/sensY as 0�100
        sensitivityXSlider.value = SensitivityToNormalized(settings.sensX);
        sensitivityYSlider.value = SensitivityToNormalized(settings.sensY);

        sensitivityXInput.text = sensitivityXSlider.value.ToString("F1");
        sensitivityYInput.text = sensitivityYSlider.value.ToString("F1");

        helmetToggle.isOn = !settings.helmetEnabled;
    }

    /// <summary>
    /// Sensitivity Handlers
    /// </summary>
    private float NormalizedToSensitivity(float normalized) => normalized * 100f;
    private float SensitivityToNormalized(float sensitivity) => sensitivity / 100f;
    private void OnSensitivityXSliderChanged(float value)
    {
        float actual = NormalizedToSensitivity(value);
        settings.sensX = actual;
        sensitivityXInput.text = value.ToString("F1"); // Display 0�1
    }

    private void OnSensitivityYSliderChanged(float value)
    {
        float actual = NormalizedToSensitivity(value);
        settings.sensY = actual;
        sensitivityYInput.text = value.ToString("F1");
    }

    private void OnSensitivityXInputChanged(string value)
    {
        if (float.TryParse(value, out float normalized))
        {
            normalized = Mathf.Clamp01(normalized);
            float actual = NormalizedToSensitivity(normalized);
            settings.sensX = actual;
            sensitivityXSlider.value = normalized;
        }
    }

    private void OnSensitivityYInputChanged(string value)
    {
        if (float.TryParse(value, out float normalized))
        {
            normalized = Mathf.Clamp01(normalized);
            float actual = NormalizedToSensitivity(normalized);
            settings.sensY = actual;
            sensitivityYSlider.value = normalized;
        }
    }
    
    private void OnHelmetToggleChange(bool value)
    {
        settings.helmetEnabled = !value;
        EventBus<HelmetSettingChange>.Raise(new HelmetSettingChange());
    }
}
