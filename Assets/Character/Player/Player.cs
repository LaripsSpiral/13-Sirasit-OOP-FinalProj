using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    [Header("Food Progress")]
    [SerializeField] private Slider _foodProgSlider;
    [SerializeField] private TMP_Text _foodProgValueTxt;
    [SerializeField] private float _foodMaxValue;
    [SerializeField] private float _foodValue;

    private void Start()
    {
        _foodProgSlider.maxValue = _foodMaxValue;
        UpdateFoodProgressBar();
    }

    public void IncreaseFoodValue(float foodAmount)
    {
        _foodValue += foodAmount;
        Debug.Log($"{this} Increase Food Value by {foodAmount}");

        UpdateFoodProgressBar();
    }

    void UpdateFoodProgressBar()
    {
        Debug.Log($"{this} Food Value is {_foodValue}");
        _foodProgSlider.value = _foodValue;
        _foodProgValueTxt.text = Mathf.FloorToInt(_foodValue / _foodMaxValue * 100) + "%";
    }
}
