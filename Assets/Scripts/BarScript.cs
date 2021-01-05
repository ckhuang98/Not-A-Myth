using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{
    public Slider slider;

    // Sets the max value of the slider.
    public void SetMaxValue(int value){
        slider.maxValue = value;
        slider.value = value;
    }

    // Sets the current value of the slider.
    public void SetValue(int value){
        slider.value = value;
    }
}
