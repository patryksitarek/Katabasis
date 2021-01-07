using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    Slider slider;

    void Start()
    {
        if (!slider)
        {
            slider = gameObject.GetComponent<Slider>();
        }
    }

    public void SetMaxHealth(float maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void SetHealth(float health)
    {
        slider.value = health;
    }

    public void ToggleOrientation()
    {
        if (slider.direction == Slider.Direction.LeftToRight)
        {
            slider.direction = Slider.Direction.RightToLeft;
        }
        else slider.direction = Slider.Direction.LeftToRight;
    }
}
