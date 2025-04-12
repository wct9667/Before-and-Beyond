using UnityEngine;

//simple class to hold reference to child images for convenience
public class UI_Ability : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image icon;
    [SerializeField] private UnityEngine.UI.Image fill;

    public UnityEngine.UI.Image Icon => icon;
    public UnityEngine.UI.Image Fill => fill;
}
