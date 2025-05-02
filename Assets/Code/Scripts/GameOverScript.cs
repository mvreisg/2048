using UnityEngine;

namespace Game.Scripts
{
    public class GameOverScript : MonoBehaviour
    {
        [SerializeField]
        private GameObject gameOverPanel;

        public void ShowGameOverPanel()
        {
            gameOverPanel.SetActive(true);
        }

        public void HideGameOverPanel()
        {
            gameOverPanel.SetActive(false);
        }
    }
}