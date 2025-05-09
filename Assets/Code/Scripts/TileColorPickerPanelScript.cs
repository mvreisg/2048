using UnityEngine;

namespace Game.Scripts
{
    public class TileColorPickerPanelScript : MonoBehaviour
    {
        [SerializeField]
        private GameObject panel;

        public void ShowPanel()
        {
            panel.SetActive(true);
        }

        public void HidePanel()
        {
            panel.SetActive(false);
        }
    }
}