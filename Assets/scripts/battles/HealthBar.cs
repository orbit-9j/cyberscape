using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{

    //https://www.youtube.com/watch?v=BLfNP4Sc_iA  06/04/2023
    public Slider slider;

    //-------
    public string sliderText;
    [SerializeField] protected TextMeshProUGUI sliderTextbox;
    //-------

    public void SetHealth(int health)
    {
        slider.value = health;
        //----
        UpdateText(health);
        //----
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        //----
        UpdateText(health);
        //-----
    }

    //-----
    private void UpdateText(int health)
    {
        sliderTextbox.text = sliderText + ": " + health + "/" + slider.maxValue;
    }
    //-----

}
