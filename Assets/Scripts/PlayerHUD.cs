using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    public Slider healthSlider = null;
    public Slider staminaSlider = null;
    public TextMeshProUGUI experienceText = null;

    private Health health = null;
    private Stamina stamina = null;
    private Experience experience = null;

    void Start()
    {
        health = GetComponent<Health>();
        stamina = GetComponent<Stamina>();
        experience = GetComponent<Experience>();

        healthSlider.minValue = 0.0f;
        healthSlider.maxValue = health.maxValue;

        staminaSlider.minValue = 0.0f;
        staminaSlider.maxValue = stamina.maxValue;

        experienceText.text = $"XP: {experience.value}";
    }

    void LateUpdate()
    {
        healthSlider.value = health.value;
        staminaSlider.value = stamina.value;
        experienceText.text = $"XP: {experience.value}";
    }
}
