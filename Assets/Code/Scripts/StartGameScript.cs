using UnityEngine;

namespace Game.Scripts
{
    public class StartGameScript : MonoBehaviour
    {
        [SerializeField]
        private GameObject startGamePanel;

        public void HideStartGamePanel()
        {
            startGamePanel.SetActive(false);
        }
    }
}