using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BillboardUIPrompt : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;
    private Canvas uIPanel;

    [SerializeField] private Image buttonPrompt;
    [SerializeField] private Sprite[] interactButtons;

    private InputDevice device;

    [SerializeField] private InputReader inputReader;
    
    const int PlayStationPrompt = 0;
    const int XboxPrompt = 1;
    const int KeyboardPrompt = 2;

    private void Awake()
    {
        uIPanel = GetComponent<Canvas>();
    }

    private bool isDisplayed;

    public bool IsDisplayed
    {
        get { return isDisplayed; }
    }

    private void Start()
    {
        uIPanel = GetComponent<Canvas>();
        uIPanel.enabled = false;
        device = inputReader.DeviceType;
    }

    /// <summary>
    /// Sets up the UI panel
    /// </summary>
    /// <param name="promptText">The text that the panel Displays</param>
    public void SetUp(string promptText)
    {
        if (promptText == "")
        {
            this.promptText.text = promptText;
            buttonPrompt.enabled = false;
        }

        else
        {
            buttonPrompt.enabled = true;
            if (device is Gamepad)
            {
                buttonPrompt.sprite = interactButtons[XboxPrompt];
            }
            else if (device is Keyboard || device is Mouse)
            {
                buttonPrompt.sprite = interactButtons[KeyboardPrompt];
            }
            else
            {
                buttonPrompt.sprite = interactButtons[PlayStationPrompt];
            }

            this.promptText.text = promptText;
        }
        uIPanel.enabled = true;
        isDisplayed = true;
    }

    /// <summary>
    /// Closes the UI
    /// </summary>
    public void Close()
    {
        uIPanel.enabled = false;
        isDisplayed = false;
    }
}