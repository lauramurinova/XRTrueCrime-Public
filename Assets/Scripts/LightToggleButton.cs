using UnityEngine;

public class ToggleSwitch : MonoBehaviour
{
    [SerializeField] private Light flashlightLight; // Reference to the Light component

    public void Toggle()
    {
        if (flashlightLight != null)
        {
            flashlightLight.enabled = !flashlightLight.enabled;
        }
    }
}