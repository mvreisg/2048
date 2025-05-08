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

        [SerializeField]
        private int winningScore;

        private Score Score { get; set; }

        private void Awake()
        {
            Score = new Score(winningScore);
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

        public bool CheckVictory(int value)
        {
            return Score.CheckVictory(value);
        }

        private void UpdateText()
        {
            actualScoreTextBox.text = Score.ActualValue.ToString();
            maxScoreTextBox.text = Score.MaxValue.ToString();
        }
    }
}