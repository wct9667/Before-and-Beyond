using System;
using UnityEngine;

[CreateAssetMenu(fileName = "settings", menuName = "settings")]
public class Settings : ScriptableObject
{
    [Header("Sensitivity")]
    [Range(0,100)] public float sensX;
    [Range(0,100)] public float sensY;

    [Header("Accessibility")] 
    public bool helmetEnabled = true;



    public void SetValues(Settings settings)
    {
        sensX = settings.sensX;
        sensY = settings.sensY;
        helmetEnabled = settings.helmetEnabled;
    }
}
