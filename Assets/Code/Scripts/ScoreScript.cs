using Game.Classes;
using TMPro;
using UnityEngine;

namespace Game.Scripts
{
    public class ScoreScript : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI actualScoreTextBox;

        [SerializeField]
        private TextMeshProUGUI maxScoreTextBox;

        private Score Score { get; set; }

        private void Awake()
        {
            Score = new Score();
            UpdateText();
        }

        public void IncreaseScoreBy(int value)
        {
            Score.IncreaseScoreBy(value);
            UpdateText();
        }

        public void ResetScore()
        {
            Score.ResetScore();
            UpdateText();
        }

        public bool CheckVictory()
        {
            return Score.CheckVictory();
        }

        private void UpdateText()
        {
            actualScoreTextBox.text = Score.ActualValue.ToString();
            maxScoreTextBox.text = Score.MaxValue.ToString();
        }
    }
}