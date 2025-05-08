using UnityEngine;

namespace Game.Scripts
{
    public class VictoryScript : MonoBehaviour
    {
        [SerializeField]
        private GameObject victoryPanel;

        public void ShowVictoryPanel()
        {
            victoryPanel.SetActive(true);
        }

        public void HideVictoryPanel()
        {
            victoryPanel.SetActive(false);
        }
    }
}