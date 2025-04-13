using UnityEngine;

[CreateAssetMenu(fileName = "settings", menuName = "settings")]
public class Settings : ScriptableObject
{
    [Range(0,100)] public float sensX;
    [Range(0,100)] public float sensY;
}
