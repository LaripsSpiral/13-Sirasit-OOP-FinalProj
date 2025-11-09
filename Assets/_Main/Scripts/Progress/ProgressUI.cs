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

        public void UpdateUI(float value)
        {
            _ProgValueTxt.text = Mathf.Clamp(value, 0, 100).ToString();
        }
    }
}