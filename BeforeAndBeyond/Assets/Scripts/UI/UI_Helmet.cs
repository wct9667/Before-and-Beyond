using UnityEngine;
using UnityEngine.UI;

public class UI_Helmet : MonoBehaviour
{
    [SerializeField] private Settings settings;
    

    private EventBinding<HelmetSettingChange> helmetSettingChangeEventBinding;
    private Image helmentImage;
    private void Awake()
    {
        helmentImage = GetComponent<Image>();
        ToggleHelmetUI();
    }

    private void OnEnable()
    {
        helmetSettingChangeEventBinding = new EventBinding<HelmetSettingChange>(ToggleHelmetUI);
        EventBus<HelmetSettingChange>.Register((helmetSettingChangeEventBinding));
    }

    private void OnDisable()
    {
        EventBus<HelmetSettingChange>.Deregister(helmetSettingChangeEventBinding);
    }

    private void ToggleHelmetUI()
    {
        helmentImage.enabled = settings.helmetEnabled;
    }
}
