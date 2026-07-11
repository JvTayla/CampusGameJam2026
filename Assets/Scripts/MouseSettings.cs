using UnityEngine;

public class MouseSettings : MonoBehaviour
{
    public float sensitivity = 2f;

    public void SetSensitivity(float value)
    {
        sensitivity = value;
        PlayerPrefs.SetFloat("Sensitivity", value);
    }

    void Start()
    {
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", 2f);
    }
}
