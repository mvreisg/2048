using Game.Classes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts
{
    public class TileScript : MonoBehaviour
    {    
        [SerializeField]
        private Image image;

        [SerializeField]
        private TextMeshProUGUI textBox;

        [SerializeField]
        private Color initialColor;

        [SerializeField]
        private Color finalColor;

        public Tile Tile { get; set; }

        private void Start()
        {            
            UpdateText();
            UpdateColor();
        }

        public void UpdateText()
        {
            textBox.text = Tile.Value.ToString();
        }

        public void UpdateColor()
        {
            textBox.color = Color.white;

            float value = Mathf.Log(Tile.Value, 2);
            float maxValue = 11;
            if (value > maxValue)
            {
                image.color = finalColor;
                return;
            }
            image.color = Color.Lerp(initialColor, finalColor, value / maxValue);
        }
    }
}