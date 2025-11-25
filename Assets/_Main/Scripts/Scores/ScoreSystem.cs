using TMPro;
using UnityEngine;

namespace Main.Score
{
    public class ScoreSystem : MonoBehaviour
    {
        public static ScoreSystem Instance;

        [SerializeField]
        private float totalScore;
        public float TotalScore => totalScore;

        [SerializeField]
        private TextMeshProUGUI hudValueLabel;

        [SerializeField]
        private TextMeshProUGUI summaryValueLabel;

        private ScoreUI scoreUI => ScoreUI.Instance;

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        public void Reset()
        {
            totalScore = 0f;
        }

        public void AddScore(float value)
        {
            totalScore += value;
            scoreUI.UpdateUI(totalScore);
        }

    }
}