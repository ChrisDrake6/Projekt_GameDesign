using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderValueToUIText : MonoBehaviour
{
    //Get Slider and Textfield
    public Slider slider;
    public TextMeshProUGUI SliderTextBox;


    //Update the Slidertext per frame
    private void Update()
    {
        SliderTextBox.text = slider.value.ToString();
    }
}
