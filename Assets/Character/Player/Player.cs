using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private Transform _heartUIParent;
    [SerializeField] private GameObject _heartPrefab;
    [SerializeField] private int _totalHeart = 3;
    [SerializeField] private int _heart;
    public int Heart
    {
        get => _heart;
        private set { _heart = Mathf.Clamp(value, 0, _totalHeart); }
    }

    [Header("Food Progress")]
    [SerializeField] private Slider _foodProgSlider;
    [SerializeField] private TMP_Text _foodProgValueTxt;
    [SerializeField] private float _foodMaxValue;
    [SerializeField] private float _foodValue;

    private void Start()
    {
        _foodProgSlider.maxValue = _foodMaxValue;
        UpdateFoodProgressBar();

        Heart = _totalHeart;
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

    public void TakeDamage(int damage)
    {
        Heart -= damage;
        UpdateHeartUI();
        Debug.Log($"{this} Taken {damage} Damage, left {Heart} Health ");

        //Chcek Death
        if (Heart > 0)
            return;

        Debug.Log($"{this} have no Health left");
    }

    void UpdateHeartUI()
    {
        //Clear Heart
        foreach (var oldHeart in _heartUIParent.GetComponentsInChildren<Image>())
        {
            Destroy(oldHeart.gameObject);
        }

        //Add Remain Heart
        for (int i = 0; i < Heart; i++)
        {
            Instantiate(_heartPrefab, _heartUIParent);
        }
    }
}
