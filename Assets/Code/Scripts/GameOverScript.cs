using UnityEngine;

namespace Game.Scripts
{
    public class GameOverScript : MonoBehaviour
    {
        [SerializeField]
        private GameObject gameOverGameObject;

        public void ShowGameOverScreen()
        {
            Debug.Log("1");
            gameOverGameObject.SetActive(true);
        }

        public void HideGameOverScreen()
        {
            Debug.Log("2");
            gameOverGameObject.SetActive(false);
        }
    }
}