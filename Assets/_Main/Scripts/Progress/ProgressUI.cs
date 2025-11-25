using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main
{
    public class ProgressUI : MonoBehaviour
    {
        [SerializeField]
        private Slider _ProgSlider;

        [SerializeField]
        private TextMeshProUGUI _ProgValueTxt;

        private float maxValue;

        public void Init(float maxValue)
        {
            this.maxValue = maxValue;
            _ProgSlider.maxValue = this.maxValue;
            UpdateUI(0);
        }

        public void UpdateUI(float value)
        {
            var percentageValue = Mathf.Clamp(value, 0, maxValue);
            _ProgSlider.value = percentageValue;
            _ProgValueTxt.text = percentageValue.ToString();
        }
    }
}