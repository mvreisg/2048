using Game.Classes;
using TMPro;
using UnityEngine;

namespace Game.Scripts
{
    public class ScoreScript : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI textBox;

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

        private void UpdateText()
        {
            textBox.text = Score.ActualValue.ToString();
        }
    }
}