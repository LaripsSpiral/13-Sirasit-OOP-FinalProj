using TMPro;
using UnityEngine;

namespace Main.Score
{
    public class ScoreUI : MonoBehaviour
    {
        public static ScoreUI Instance;

        [SerializeField]
        private TextMeshProUGUI hudValueLabel;

        [SerializeField]
        private TextMeshProUGUI summaryValueLabel;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            UpdateUI(ScoreSystem.Instance.TotalScore);
        }

        public void UpdateUI(float score)
        {
            var displayValue = score.ToString("0");
            hudValueLabel.text = displayValue;
            summaryValueLabel.text = displayValue;
        }
    }
}