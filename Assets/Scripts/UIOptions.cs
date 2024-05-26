using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIOptions : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Slider slider;
    [SerializeField] private Image fadeImage;

    private float minValue = 0;
    private float maxValue = 0.95f;

    void Awake()
    {
        button.onClick.AddListener(ButtonExampleTwo);

        slider.minValue = minValue;
        slider.maxValue = maxValue;
        slider.value = maxValue;
    }

    void Update()
    {
        float alphaValue = 0.95f - slider.value;

        Color newColor = fadeImage.color;

        newColor.a = alphaValue;
        fadeImage.color = newColor;
    }

    public void ButtonExampleOne()
    {
        Debug.Log("El botón se ha activado");
    }

    public void ButtonExampleTwo()
    {
        Debug.Log("El botón secundario se ha activado");
    }
}
