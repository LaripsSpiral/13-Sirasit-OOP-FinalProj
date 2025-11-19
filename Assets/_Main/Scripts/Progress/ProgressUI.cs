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

        private void Start()
        {
            _ProgSlider.maxValue = 100;
            UpdateUI(0);
        }

        public void UpdateUI(float value)
        {
            var percentageValue = Mathf.Clamp(value, 0, 100);

            _ProgSlider.value = percentageValue;
            _ProgValueTxt.text = percentageValue.ToString();
        }
    }
}