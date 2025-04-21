using System;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [Header("UI")] [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Canvas settingsCanvas;

    [Header("Buttons-MainCanvas")] [SerializeField]
    private UnityEngine.UI.Button buttonStart;

    [SerializeField] private UnityEngine.UI.Button buttonSettings;
    [SerializeField] private UnityEngine.UI.Button buttonQuit;

    [Header("Buttons-SettingsCanvas")] [SerializeField]
    private UnityEngine.UI.Button buttonSettingsBack;

    [Header("Scene Index for starting game scene")] [SerializeField]
    private int startSceneIndex;

    [SerializeField] private InputReader inputReader;
    
    private void OnEnable()
    {
        inputReader.SetUIInputMap();
        //setup OnClicks for buttons
        buttonStart.onClick.AddListener(() => { SceneLoader.LoadScene(startSceneIndex); });

        buttonQuit.onClick.AddListener(SceneLoader.QuitGame);

        buttonSettings.onClick.AddListener(() =>
        {
            mainCanvas.enabled = false;
            settingsCanvas.enabled = true;
            buttonSettingsBack.Select();
        });

        buttonSettingsBack.onClick.AddListener(() =>
        {
            mainCanvas.enabled = true;
            settingsCanvas.enabled = false;
            buttonStart.Select();
        });
    }

    private void OnDisable()
    {
        buttonStart.onClick.RemoveAllListeners();
        buttonQuit.onClick.RemoveAllListeners();
        buttonSettings.onClick.RemoveAllListeners();
        buttonSettingsBack.onClick.RemoveAllListeners();
    }
}